(**

- title : Crunching big data in Azure with MBrace
- description : Crunching big data in Azure with MBrace
- author : Magnus Härlin
- theme : moon
- transition : default

***

*)

(*** hide ***)
#load "../packages/MBrace.Azure/MBrace.Azure.fsx"
#load "../packages/MBrace.Azure.Management/MBrace.Azure.Management.fsx"
#r "../packages/MBrace.Flow/lib/net45/MBrace.Flow.dll"


//let config = Unchecked.defaultof<MBrace.Azure.Configuration>

open System
open MBrace.Core
open MBrace.Azure
open MBrace.Azure.Management

(**

<br /><br />

## Crunching big data in Azure with MBrace

<br /><br />

- Magnus Härlin
- SpeedLedger
- @MagnusHarlin
- github/mharlin

' Kodexempel i F#
' Min F# resa. Hemma hack -> Open source projekt -> byggscript
' F#, minskad kodbas, klasser kan inte vara null, immutability

***

### Agenda

- Vad är MBrace
- Hur konfigurerar man MBrace-kluster
- Cloud computation expression
- CPU-intensivt arbete - räkna ut primtal
- Bearbeta data
- Machine learning - läsa av handskrivna siffror
  
***

### Vad är MBrace

- Passar bra till mycket data eller tunga beräkningar
- Helt Open Source
- Jobba direkt från editorn - prototypning
- Som separata script eller inbyggt i en applikation
- Konfigurera infrastrukturen med kod

' Lagra data online så att man inte behöver ladda upp det flera gånger

***

### Vad kan man parallellisera

- Immutable
- Inga sidoeffekter

' Linq med where och select och Seq med map och filter

***

### Cluster with code

*)

let clusterName = "swetugg-mbrace-cluster"

let GetSubscriptionManager() = 
    SubscriptionManager.FromPublishSettingsFile("my.publishSettings", 
                                                Region.North_Europe, 
                                                ?subscriptionId = Some "abc123", 
                                                logger = new ConsoleLogger())

let GetDeployment() = 
    GetSubscriptionManager().GetDeployment(clusterName) 

let ProvisionCluster() = 
    GetSubscriptionManager().Provision(vmCount = 4, 
                                        serviceName = clusterName, 
                                        vmSize = VMSize.A2)

let GetCluster() = 
    AzureCluster.Connect(GetDeployment(),
                         logger = ConsoleLogger(true),
                         logLevel = LogLevel.Info)

(**
' publish settings can be found in the azure portal

---

### Using your Azure cluster from compiled code

Add a reference to the ``MBrace.Azure`` package

' The configuration strings can be found by the
' Azure management console or the ``GetDeployment().Configuration`` in
' an MBrace data script.

*)

let storageConnectionString = "..."
let serviceBusConnectionString = "..."

let config = Configuration(storageConnectionString, 
                           serviceBusConnectionString)

let cluster = AzureCluster.Connect(config, 
                                   logger = ConsoleLogger(true), 
                                   logLevel = LogLevel.Info)
(**

***

### Demo - Hello World in the cloud

***

### C#

    [lang=cs]
    var cluster = Config.GetCluster();

    var text = cluster.Run(CloudBuilder.FromFunc(() => "Hello, World!"));

***

### Combinators

- Cloud.Parallel(computations)
- Cloud.OfAsync(computations)
- Cloud.Ignore(workflow)
- Cloud.Choice(computations)
- ...

***

### Cloud Flow

- CloudFlow.OfArray
- CloudFlow.OfCloudFileByLine
- CloudFlow.OfCloudQueue
- CloudFlow.OfSeqs
- ...

---

$ filter \rightarrow map \rightarrow sort \rightarrow  aggragate $ 

***

### Cloud Storage

- Values
- Files
- Queues
- KeyValueStores

***

### Demo - Räkna ut primtal och bearbeta data

***

### Exceptions and Fault Tolerance

*)

cloud { do failwith "kaboom!" } |> cluster.Run

(**

---

*)

cloud { do failwith "kaboom!" } |> cluster.Run

(**

    [lang=console] 
    System.Exception: kaboom!
    at FSI_0010.it@24-3.Invoke(Unit unitVar)
    at MBrace.Core.BuilderImpl.Invoke@98.Invoke(Continuation`1 c) in ...
    --- End of stack trace from previous location where exception was thrown ---
    at <StartupCode$MBrace-Runtime>.$CloudProcess.AwaitResult@211-2.Invoke(CloudProcessResult _arg2) in ...
    at Microsoft.FSharp.Control.AsyncBuilderImpl.args@835-1.Invoke(a a)
    at MBrace.Core.Internals.AsyncExtensions.Async.RunSync[T](FSharpAsync`1 workflow, ...
    at <StartupCode$FSI_0010>.$FSI_0010.main@() in ...
    Stopped due to error

' Felet fångas på serversidan och kastas om på klienten
' MBrace stödjer felhantering på samma sätt som async
' Ohanterade fel som hamnar hos föräldern gör att alla beräkningar kommer att stoppas

---

*)

cloud {
    let div m n = cloud { return m / n }
    let! results = Cloud.Parallel [for i in 1 .. 10 -> div 10 (5 - i) ]
    return Array.sum results
}

(**

---

*)

cloud {
    let div m n = cloud { return m / n }
    let! results = Cloud.Parallel [for i in 1 .. 10 -> div 10 (5 - i) ]
    return Array.sum results
}

(**

    [lang=console]
    System.DivideByZeroException: Attempted to divide by zero.
       at FSI_0013.div@47-1.Invoke(Unit unitVar)
       at MBrace.Core.BuilderImpl.Invoke@98.Invoke(Continuation`1 c) in ...
       at Cloud.Parallel(seq<Cloud<Int32>> computations)
    --- End of stack trace from previous location where exception was thrown ---
       at <StartupCode$MBrace-Runtime>.$CloudProcess.AwaitResult@211-2.Invoke(CloudProcessResult _arg2) in ...
       at Microsoft.FSharp.Control.AsyncBuilderImpl.args@835-1.Invoke(a a)
       at MBrace.Core.Internals.AsyncExtensions.Async.RunSync[T](FSharpAsync`1 ...
       at <StartupCode$FSI_0014>.$FSI_0014.main@()

---

### Vettiga felmeddelanden
*)

exception WorkerException of worker:IWorkerRef * input:int * exn:exn
with
    override e.Message = 
        sprintf "Worker '%O' given input %d has failed with exception:\n'%O'" 
            e.worker e.input e.exn


cloud {
    let div n = cloud { 
        try return 10 / (5 - n)
        with e -> 
            let! currentWorker = Cloud.CurrentWorker
            return raise (WorkerException(currentWorker, n, e))
    }

    let! results = Cloud.Parallel [for i in 1 .. 10 -> div i ]
    return Array.sum results
}

(**

---

    [lang=console]
    FSI_0023+WorkerException: Worker 'mbrace://magnus:52789' given input 5 has failed with exception: 
    'System.DivideByZeroException: Attempted to divide by zero.
       at FSI_0024.div@88-32.Invoke(Unit unitVar)
       at MBrace.Core.BuilderImpl.Invoke@98.Invoke(Continuation`1 c) in ...'
       at FSI_0024.div@91-34.Invoke(IWorkerRef _arg2)
       at MBrace.Core.Builders.Bind@331-1.Invoke(ExecutionContext ctx, T t) in ...
       at Cloud.Parallel(seq<Cloud<Int32>> computations)
    --- End of stack trace from previous location where exception was thrown ---
       at <StartupCode$MBrace-Runtime>.$CloudProcess.AwaitResult@211-2.Invoke(CloudProcessResult _arg2) in ...
       at Microsoft.FSharp.Control.AsyncBuilderImpl.args@835-1.Invoke(a a)
       at MBrace.Core.Internals.AsyncExtensions.Async.RunSync[T](FSharpAsync`1 workflow, FSharpOption`1 ...
       at <StartupCode$FSI_0025>.$FSI_0025.main@()

---

### Hantera fel
*)

cloud {
    let div m n = cloud { return m / n }
    try
        let! results = Cloud.Parallel [for i in 1 .. 10 -> div 10 (5 - i) ]
        return Some(Array.sum results)
    with :? DivideByZeroException ->
        return None
}

(**

---

### Felen som en del av resultatet

*)

cloud {
    let div m n = cloud { try return Choice1Of2(m / n) with e -> return Choice2Of2 e }
    let! results = Cloud.Parallel [for i in 1 .. 10 -> div 10 (5 - i) ]
    return results
}

(**

---

    [lang=console]
    val it : Choice<int,exn> [] =
      [|Choice1Of2 2; Choice1Of2 3; Choice1Of2 5; Choice1Of2 10;
        Choice2Of2
          System.DivideByZeroException: Attempted to divide by zero.
       at FSI_0031.div@140-47.Invoke(Unit unitVar)
       at MBrace.Core.BuilderImpl.Invoke@98.Invoke(Continuation`1 c) in ...
            {Data = dict [];
             HResult = -2147352558;
             HelpLink = null;
             InnerException = null;
             Message = "Attempted to divide by zero.";
             Source = "FSI-ASSEMBLY_f0c42c06-f5a8-45d0-ab7b-2fec2628dff0_10";
             StackTrace = "   at FSI_0031.div@140-47.Invoke(Unit unitVar)"
       at MBrace.Core.BuilderImpl.Invoke@98.Invoke(Continuation`1 c) in ...;
             TargetSite = null;}; Choice1Of2 -10; Choice1Of2 -5; Choice1Of2 -3;
        Choice1Of2 -2; Choice1Of2 -2|]

***

### Demo: KNN - läsa av handskrivna siffror

![training data](images/knn-images.png)

---

### Demo: K-means clustering

![K-mean clustering](images/K-means_convergence.png)

***

### Slutsatser
- Skalbart - power on demand
- Fantastiskt för prototypning
- Snabbt att komma igång med

***

### Tack för att ny lyssnade!
<br />

- [github.com/Mharlin/Demon/tree/master/Swetugg2017](https://github.com/Mharlin/Demon/tree/master/Swetugg2017)
- @MagnusHarlin
*)