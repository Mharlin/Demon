//#load "ThespianCluster.fsx"
#load "AzureCluster.fsx"


open System
open System.IO
open MBrace.Core
open MBrace.Flow

let cluster = Config.GetCluster() 

#load "lib/utils.fsx"
#load "lib/sieve.fsx"

let locallyComputedPrimes =
    [| for i in 1 .. 10 ->
         let primes = Sieve.getPrimes 300000000
         sprintf "calculated %d primes: %A" primes.Length primes  |]




cluster.ShowProcesses()
cluster.ShowWorkers()

//let jobResults = 
//    [ for job in jobs -> job.Result ]
//
//let jobTimes = 
//    [ for job in jobs -> job.ExecutionTime ]











//let jobs =
//    [| for i in 1 .. 10 ->
//        cloud {
//            let primes = Sieve.getPrimes 300000000
//            return sprintf "calculated %d primes: %A on machine: %s" primes.Length primes Environment.MachineName
//        }
//        |> cluster.CreateProcess |] 

//let jobResults = 
//    [ for job in jobs -> job.Result ]
//
//let jobTimes = 
//    [ for job in jobs -> job.ExecutionTime ]

