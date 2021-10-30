using System;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;

namespace blobPropertiesAndMetadata
{
    public class Common
    {
        public static CloudBlobClient CreateBlobClientStorageFromSAS(string SASToken, string accountName)
        {
            CloudStorageAccount storageAccount;
            CloudBlobClient blobClient;
            try
            {
                bool useHttps = true;
                StorageCredentials storageCredentials = new StorageCredentials(SASToken);
                storageAccount = new CloudStorageAccount(storageCredentials, accountName, null, useHttps);
                blobClient = storageAccount.CreateCloudBlobClient();
            }
            catch (System.Exception)
            {
                throw;
            }
            return blobClient;
        }
    }
}