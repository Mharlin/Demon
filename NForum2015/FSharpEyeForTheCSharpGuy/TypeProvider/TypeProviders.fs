module JsonParse 

open FSharp.Charting
open FSharp.Data

//Access by strongly typed object by property name
type Simple = JsonProvider<""" { "name": "Magnus", "age":35 }  """>
let simple = Simple.Parse(""" { "name": "Tove", "age": 4 } """ )

//Type inferred
type Numbers = JsonProvider<""" [1, 2, 3, 3.14] """>
let nums = Numbers.Parse(""" [2, 1.2, 45.1, 98.2, 5] """)

let total = nums |> Seq.sum

//Mixed types
type Mixed = JsonProvider<""" [1, 2, "hello", "world"] """>
let mixed = Mixed.Parse(""" [4, "hello", "world", 5 ] """)

let sum = mixed.Numbers |> Seq.sum
let strings = mixed.Strings |> String.concat ", "




//GitHub issues
//    type GitHub = JsonProvider<"https://api.github.com/repos/fsharp/FSharp.Data/issues">
type GitHub = JsonProvider<"https://api.github.com/repos/Mharlin/Demon/issues">

let topRecentlyUpdatedIssues = 
    GitHub.GetSamples()
    |> Seq.filter (fun issue -> issue.State = "open")
    |> Seq.sortBy (fun issue -> System.DateTime.Now - issue.UpdatedAt)
    |> Seq.truncate 5

for issue in topRecentlyUpdatedIssues do
    printfn "#%d %s" issue.Number issue.Title

// #load "../packages/FSharp.Charting.0.90.9/FSharp.Charting.fsx";;
//World bank
type WorldBank = WorldBankDataProvider<Asynchronous=true>
let data = WorldBank.GetDataContext()
let chart = 
    data.Countries.Sweden
        .Indicators.``Population, total``
        |> Async.RunSynchronously
        |> Chart.Line
   
