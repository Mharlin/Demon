(*** hide ***)
//#load "ThespianCluster.fsx"
#load "AzureCluster.fsx"


open System
open System.IO
open MBrace.Core
open MBrace.Flow

let cluster = Config.GetCluster() 

#load "lib/sieve.fsx"


let inputs = [| 1..100 |]

let streamComputationTask = 
    inputs
    |> CloudFlow.OfArray
    |> CloudFlow.map (fun num -> num * num)
    |> CloudFlow.map (fun num -> num % 10)
    |> CloudFlow.countBy id
    |> CloudFlow.toArray
    |> cluster.CreateProcess

streamComputationTask.ShowInfo()

streamComputationTask.Result


let numbers = [| for i in 1 .. 30 -> 50000000 |]

let computePrimesTask = 
    numbers
    |> CloudFlow.OfArray
    |> CloudFlow.withDegreeOfParallelism 6
    |> CloudFlow.map (fun n -> Sieve.getPrimes n)
    |> CloudFlow.map (fun primes -> sprintf "calculated %d primes: %A" primes.Length primes)
    |> CloudFlow.toArray
    |> cluster.CreateProcess 

computePrimesTask.ShowInfo()

let computePrimes = computePrimesTask.Result

let persistedCloudFlow =
    inputs
    |> CloudFlow.OfArray
    |> CloudFlow.collect(fun i -> seq {for j in 1 .. 10000 -> (i+j, string j) })
    |> CloudFlow.persist StorageLevel.Memory
    |> cluster.Run


let length = persistedCloudFlow |> CloudFlow.length |> cluster.Run
let max = persistedCloudFlow |> CloudFlow.maxBy fst |> cluster.Run

