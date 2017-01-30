//#load "../ThespianCluster.fsx"
#load "../AzureCluster.fsx"

open System
open System.IO
open MBrace.Core
open MBrace.Flow

let cluster = Config.GetCluster() 

let atom = CloudAtom.New(100) |> cluster.Run

atom.Id

let atomValue = cloud { return atom.Value } |> cluster.Run

let atomUpdateResult = cloud { return atom.Transact(fun x -> string x,x*x) } |> cluster.Run

cloud {
    let! clusterSize = Cloud.GetWorkerCount()
    let! _ = Cloud.Parallel [ for i in 1 .. clusterSize * 2 -> cloud { atom.Update(fun i -> i + 1) } ]
    return atom.Value
} |> cluster.Run

atom.Dispose() |> Async.RunSynchronously

cluster.ShowProcesses()

