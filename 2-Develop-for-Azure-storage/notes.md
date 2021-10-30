# Develop for Azure storage

## 01-move-storage-blobs-c-sharp

C# code to move a blob from one Storage Account to another

## 02-blob-lifecycle-management-policy

A policy that applies to all blobs under the container named container-a, as stated by the prefixMatch in the filters section. Actions are as follows:

* Blobs not modified for greater than 30 days are moved to the cool tier
* Blobs not modified for greater than 90 days are moved to the archive tier
* Blobs not modified for greater than 2,555 days are deleted from the Storage Account

## 03-change-blob-access-tier-c-sharp
