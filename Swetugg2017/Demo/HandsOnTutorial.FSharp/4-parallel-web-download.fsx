//#load "ThespianCluster.fsx"
#load "AzureCluster.fsx"

open System
open System.IO
open MBrace.Core
open MBrace.Flow


#load "lib/utils.fsx"

open System.Net

let cluster = Config.GetCluster() 

let urls = 
    [| ("SpeedLedger", "http://speedledger.se")
       ("aftonbladet", "http://aftonbladet.se")
       ("expressen", "http://expressen.se")
       ("dn", "http://dn.se")
       ("svd", "http://svd.se") |]

let download (name: string, uri: string) = 
        let webClient = new WebClient()
        let text = webClient.AsyncDownloadString(Uri(uri)) |> Async.RunSynchronously
        (name, text)

let countRows text = 
    text
    |> String.filter (fun c -> c = '\n')
    |> String.length

let countLengthTask = 
    urls 
    |> CloudFlow.OfArray
    |> CloudFlow.map download
//    |> CloudFlow.sumByKey fst (snd >> countRows)
    |> CloudFlow.sumByKey (fun (name, text) -> name) (fun (name, text) -> (countRows text))
    |> CloudFlow.toArray
    |> cluster.CreateProcess

countLengthTask.ShowInfo()

let files = countLengthTask.Result

