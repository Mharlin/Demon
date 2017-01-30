//#load "ThespianCluster.fsx"
#load "AzureCluster.fsx"

open System
open System.Numerics
open System.IO
open System.Text

open Nessos.Streams

open MBrace.Core
open MBrace.Flow

let cluster = Config.GetCluster() 

[<Literal>]
let pixelLength = 784 // 28 * 28

type ImageId = int

type Image = 
    { Id : ImageId 
      Pixels : int [] }

    static member Parse file=
        File.ReadAllLines file
        |> Stream.ofSeq
        |> Stream.skip 1
        |> Stream.map (fun line -> line.Split(','))
        |> Stream.map (fun line -> line |> Array.map int)
        |> Stream.mapi (fun i nums -> let id = i + 1 in { Id = id ; Pixels = nums })
        |> Stream.toArray

type Classification = int

type Distance = Image -> Image -> uint64

type TrainingImage = 
    { Classification : Classification 
      Image : Image }

    static member Parse file =
        File.ReadAllLines file
        |> Stream.ofSeq
        |> Stream.skip 1
        |> Stream.map (fun line -> line.Split(','))
        |> Stream.map (fun line -> line |> Array.map int)
        |> Stream.mapi (fun i nums -> 
                            let id = i + 1
                            let image = { Id = id ; Pixels = nums.[1..] }
                            { Classification = nums.[0] ; Image = image })
        |> Stream.toArray

type Classifier = TrainingImage [] -> Image -> Classification

let l2 : Distance =
    fun x y ->
        let xp = x.Pixels
        let yp = y.Pixels
        let mutable acc = 0uL
        for i = 0 to pixelLength - 1 do
            acc <- acc + uint64 (pown (xp.[i] - yp.[i]) 2)
        acc

let knn (d : Distance) (k : int) : Classifier =
    fun (training : TrainingImage []) (img : Image) ->
        training
        |> Stream.ofArray
        |> Stream.sortBy (fun ex -> d ex.Image img)
        |> Stream.take k
        |> Stream.map (fun ex -> ex.Classification)
        |> Stream.countBy id
        |> Stream.maxBy snd
        |> fst


let classifyLocalMulticore (classifier : Classifier) (training : TrainingImage []) (images : Image []) =
    ParStream.ofArray images
    |> ParStream.map (fun img -> img.Id, classifier training img)
    |> ParStream.toArray

let validateLocalMulticore (classifier : Classifier) (training : TrainingImage []) (validation : TrainingImage []) =
    ParStream.ofArray validation
    |> ParStream.map(fun tr -> tr.Classification, classifier training tr.Image)
    |> ParStream.map(fun (expected,prediction) -> if expected = prediction then 1. else 0.)
    |> ParStream.sum
    |> fun results -> results / float validation.Length



//// Performance (3.5Ghz Quad Core i7 CPU)
//// Real: 00:01:02.281, CPU: 00:07:51.481, GC gen0: 179, gen1: 82, gen2: 62
//validateLocalMulticore classifier training.[ .. 39999] training.[40000 ..]
//
//// Performance (3.5Ghz Quad Core i7 CPU)
//// Real: 00:15:30.855, CPU: 01:56:59.842, GC gen0: 2960, gen1: 2339, gen2: 1513
//classifyLocalMulticore classifier training tests


let classifyCloud (classifier : Classifier) (training : TrainingImage []) (images : Image []) =
    CloudFlow.OfArray images
    |> CloudFlow.map (fun img -> img.Id, classifier training img)
    |> CloudFlow.toArray

let validateCloud (classifier : Classifier) (training : TrainingImage []) (validation : TrainingImage []) = cloud {
    let! successCount =
        CloudFlow.OfArray validation
        |> CloudFlow.filter (fun tI -> classifier training tI.Image = tI.Classification)
        |> CloudFlow.length

    return float successCount / float validation.Length
}


let trainPath = __SOURCE_DIRECTORY__ + "/../data/train.csv"
let testPath = __SOURCE_DIRECTORY__ + "/../data/test.csv"

let training = TrainingImage.Parse trainPath
let tests = Image.Parse testPath

let classifier = knn l2 10

#time

let validateTask = 
    cloud { return! validateCloud classifier training.[0 .. 39999] training.[40000 ..] } 
    |> cluster.CreateProcess

cluster.ShowWorkers()
validateTask.ShowInfo()

let classifyTask = cloud { return! classifyCloud classifier training tests } |> cluster.CreateProcess

cluster.ShowWorkers()
classifyTask.ShowInfo()

let validateResult = validateTask.Result
let classifyResult = classifyTask.Result


/// original clissification with l2 and k = 10 -- accuracy = 0.9635

let solutions = cloud {
    let validateOptions = 
        [for k in 1 .. 10 -> cloud {
                let classifier = knn l2 k
                let correctlyClassified = validateLocalMulticore classifier training.[0 .. 39999] training.[40000 ..] 
                return (k, correctlyClassified) } ] 
        |> Cloud.Parallel  
    return! validateOptions } |> cluster.CreateProcess

cluster.ShowWorkers()
solutions.ShowInfo()

solutions.Result

