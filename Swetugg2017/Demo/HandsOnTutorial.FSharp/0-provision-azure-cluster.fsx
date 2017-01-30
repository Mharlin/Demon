#load "AzureCluster.fsx"

open MBrace.Azure
open MBrace.Azure.Management



let deployment = Config.ProvisionCluster() // Tar ca 5 min


deployment.ShowInfo()


let cluster = Config.GetCluster()

cluster.ShowWorkers()

Config.ResizeCluster 10


Config.DeleteCluster()

