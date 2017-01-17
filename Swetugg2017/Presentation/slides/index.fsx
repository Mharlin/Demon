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
- Bearbeta stora datamängder
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

### Cluster with BriskEngine

---

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

' Flödet börjar med filter -> sort -> map -> aggragate

---

filter-->sort; 

***

### Cloud Storage

- Atoms
- Values
- Files
- Queues
- KeyValueStores
- ...

' Queues - reactive proramming och när allt data inte finns tillgängligt från början
' CloudAtom - dela mutable data mellan noderna

*)