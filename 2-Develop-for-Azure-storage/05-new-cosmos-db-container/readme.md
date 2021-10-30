# Create a New Cosmos DB Container

## Azure CLI

```bash
rgName='az204-rg'
accountName='NameOfAccount'
dbName='NameOfDatabase'
containerName='ContainerName'
partitionKey='/PartitionKey'
throughput=400

az cosmosdb sql container create \
    -a $accountName \
    -g $rgName \
    -d $dbName \
    -n $containerName \
    -p $partitionKey \
    --throughput $throughput
```

## Az PowerShell

```powershell
$rgName = 'az204-rg'
$accountName = 'NameOfAccount'
$dbName = 'NameOfDatabase'
$containerName = 'ContainerName'
$partitionKey = '/PartitionKey'
$throughput = 400

New-AzCosmosDBSqlContainer `
-ResourceGroupName $rgName `
-AccountName $accountName `
-DatabaseName $dbName `
-Name $containerName `
-PartitionKeyKing Hash `
-PartitionKeyPath $partitionKey `
-Throughput $throughput
```
