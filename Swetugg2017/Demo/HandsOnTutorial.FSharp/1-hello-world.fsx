#load "ThespianCluster.fsx"
//#load "AzureCluster.fsx"

open System
open MBrace.Core


let cluster = Config.GetCluster()
cluster.ShowWorkers()


let helloWorld = 2 + 2













//let helloWorldAsync = 
//    async { return 2 + 2 }
//    |> Async.RunSynchronously
//
//
//let helloWorldCloud = 
//    cloud { return 2 + 2 }
//    |> cluster.Run
//
//let helloWorldTask = 
//    cloud { return 2 + 2 } 
//    |> cluster.CreateProcess
//
//helloWorldTask.ShowInfo()
//
//let text = helloWorldTask.Result


cluster.ShowProcesses()

cluster.ClearAllProcesses()
