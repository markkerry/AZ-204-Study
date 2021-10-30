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