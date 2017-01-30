//#load "../ThespianCluster.fsx"
#load "../AzureCluster.fsx"

#load "../lib/utils.fsx"
#load "../../packages/FSharp.Charting/FSharp.Charting.fsx"
#r "../../packages/FSharp.Control.AsyncSeq/lib/net45/FSharp.Control.AsyncSeq.dll"

open System
open Nessos.Streams
open MBrace.Core
open FSharp.Charting

let cluster = Config.GetCluster() 

let dim = 2 
let numCentroids = 5 
let partitions = 12 
let pointsPerPartition = 50000 
let epsilon = 0.1

type Point = float[]

let generatePoints dim numPoints seed : Point[] =
    let rand = Random(seed * 2003 + 22)
    let prev = Array.zeroCreate dim

    let nextPoint () =
        let arr = Array.zeroCreate dim
        for i = 0 to dim - 1 do 
            arr.[i] <- prev.[i] + rand.NextDouble() * 40.0 - 20.0
            prev.[i] <- arr.[i]
        arr

    [| for i in 1 .. numPoints -> nextPoint() |]

let randPoints = Array.init partitions (generatePoints dim pointsPerPartition)

let point2d (p:Point) = p.[0], p.[1]

let selectionOfPoints = 
    [ for points in randPoints do 
         for i in 0 .. 100 .. points.Length-1 do
             yield point2d points.[i] ]

Chart.Point selectionOfPoints 


[<AutoOpen>]
module KMeansHelpers =

    let dist (p1 : Point) (p2 : Point) = 
        Array.fold2 (fun acc e1 e2 -> acc + pown (e1 - e2) 2) 0.0 p1 p2

    let findCentroid (p: Point) (centroids: Point[]) : int =
        let mutable mini = 0
        let mutable min = Double.PositiveInfinity
        for i = 0 to centroids.Length - 1 do
            let dist = dist p centroids.[i]
            if dist < min then
                min <- dist
                mini <- i

        mini

    let kmeansLocal (points : Point[]) (centroids : Point[]) : (int * (int * Point))[] =
        let lens = Array.zeroCreate centroids.Length
        let sums = 
            Array.init centroids.Length (fun _ -> Array.zeroCreate centroids.[0].Length)

        for point in points do
            let cent = findCentroid point centroids
            lens.[cent] <- lens.[cent] + 1
            for i = 0 to point.Length - 1 do
                sums.[cent].[i] <- sums.[cent].[i] + point.[i]

        Array.init centroids.Length (fun i -> (i, (lens.[i], sums.[i])))

    let sumPoints (pointArr : Point []) dim : Point =
        let sum = Array.zeroCreate dim
        for p in pointArr do
            for i = 0 to dim - 1 do
                sum.[i] <- sum.[i] + p.[i]
        sum

    let divPoint (point : Point) (x : float) : Point =
        Array.map (fun p -> p / x) point

let rec KMeansCloudIterate (partitionedPoints, epsilon, centroids, iteration, emit) = cloud {

    let! clusterParts =
        partitionedPoints
        |> Array.map (fun (p:CloudArray<_>, w) -> cloud { return kmeansLocal p.Value centroids }, w)
        |> Cloud.ParallelOnSpecificWorkers

    let dim = centroids.[0].Length
    let newCentroids =
        clusterParts
        |> Array.concat
        |> ParStream.ofArray
        |> ParStream.groupBy fst
        |> ParStream.sortBy fst
        |> ParStream.map snd
        |> ParStream.map (fun clp -> clp |> Seq.map snd |> Seq.toArray |> Array.unzip)
        |> ParStream.map (fun (ns,points) -> Array.sum ns, sumPoints points dim)
        |> ParStream.map (fun (n, sum) -> divPoint sum (float n))
        |> ParStream.toArray

    let diff = Array.map2 dist newCentroids centroids |> Array.max

    do! Cloud.Logf "KMeans: iteration [#%d], diff %A with centroids /n%A" iteration diff centroids

    emit(DateTimeOffset.UtcNow,iteration,diff,centroids)

    if diff < epsilon then
        return newCentroids
    else
        return! KMeansCloudIterate (partitionedPoints, epsilon, newCentroids, iteration+1, emit)
}

            
let KMeansCloud(points, numCentroids, epsilon, emit) = cloud {  

    let initCentroids = points |> Seq.concat |> Seq.take numCentroids |> Seq.toArray

    let! workers = Cloud.GetAvailableWorkers()
    do! Cloud.Logf "KMeans: persisting partitioned point data to store."
        
    let! partitionedPoints = 
        points 
        |> Seq.mapi (fun i p -> 
            local { 
                let! ca = CloudValue.NewArray(p, StorageLevel.MemoryAndDisk) 
                return ca, workers.[i % workers.Length] }) 
        |> Local.Parallel

    do! Cloud.Logf "KMeans: persist completed, starting iteration."

    return! KMeansCloudIterate(partitionedPoints, epsilon, initCentroids, 1, emit) 
}


// Running a test flight of the algorithm 
let kmeansTask = 
    KMeansCloud(randPoints, numCentroids, epsilon*10000.0, ignore) 
    |> cluster.CreateProcess


cluster.ShowWorkers()
cluster.ShowProcesses()
kmeansTask.ShowLogs()
kmeansTask.ShowInfo()

let centroids = kmeansTask.Result

Chart.Combine   
    [ Chart.Point(selectionOfPoints)
      Chart.Point(centroids |> Array.map point2d, Color=Drawing.Color.Red) ]





type Observation = DateTimeOffset * int * float * Point[]

let watchQueue =  CloudQueue.New<Observation>()  |> cluster.RunLocally

let kmeansTask2 = 
    KMeansCloud(randPoints, numCentroids, epsilon, watchQueue.Enqueue) 
    |> cluster.CreateProcess

kmeansTask2.ShowLogs()
cluster.ShowWorkers()
cluster.ShowProcesses()
kmeansTask2.ShowInfo()


open FSharp.Control

asyncSeq { 
    let centroidsSoFar = ResizeArray()
    while true do
        match watchQueue.TryDequeue() with
        | Some (time, iteration, diff, centroids) -> 
                centroidsSoFar.Add centroids
                let d = [ for centroids in centroidsSoFar do for p in centroids -> point2d p ]
                yield d
                do! Async.Sleep 1000
        | None -> do! Async.Sleep 1000
} |> AsyncSeq.toObservable |> LiveChart.Point 



let centroids2 = kmeansTask2.Result

Chart.Combine   
    [ Chart.Point(selectionOfPoints)
      Chart.Point(centroids |> Array.map point2d, Color=Drawing.Color.Orange,MarkerSize=10) 
      Chart.Point(centroids2 |> Array.map point2d, Color=Drawing.Color.Red,MarkerSize=5) ]

