using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Azure;
using Azure.Identity;
using System.IO;

namespace createUserDelegationSasToken
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageAccount = "";
            string containerName = "az204-blob-test";
            string blobName = System.IO.Path.GetRandomFileName();

            DateTimeOffset startTimeKey = DateTimeOffset.UtcNow;
            DateTimeOffset endTimeKey = DateTimeOffset.UtcNow.AddDays(7);
            DateTimeOffset startTimeSAS = startTimeKey;
            DateTimeOffset endTimeSAS = startTimeKey.AddDays(1);

            Uri blobEndpointUri = new Uri($"https://{storageAccount}.blob.core.windows.net");

            var defaultCredentials = new DefaultAzureCredential(true);

            BlobServiceClient blobClient = new BlobServiceClient(blobEndpointUri, defaultCredentials);

            // Get the key to use for the SAS
            UserDelegationKey key = blobClient.GetUserDelegationKey(startTimeKey, endTimeKey);

            System.Console.WriteLine($"User key starts on: {key.SignedStartsOn}");
            System.Console.WriteLine($"User key Expires on: {key.SignedExpiresOn}");
            System.Console.WriteLine($"User key service: {key.SignedService}");
            System.Console.WriteLine($"User key version: {key.SignedVersion}");

            // Use the BlobSasBuilder for creating the SAS
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = startTimeSAS,
                ExpiresOn = endTimeSAS,
                Protocol = Azure.Storage.Sas.SasProtocol.Https
            };

            // Set the permissions to Create, List, Add, Read, and Write
            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.All);

            string sasToken = blobSasBuilder.ToSasQueryParameters(key, storageAccount).ToString();

            System.Console.WriteLine($"SAS Token: {sasToken}");

            // Construct the full uri for accessing the Azure Storage Account
            UriBuilder blobUri = new UriBuilder()
            {
                Scheme = "https",
                Host = $"{storageAccount}.blob.core.windows.net",
                Path = $"{containerName}/{blobName}",
                Query = sasToken
            };

            // Create a random text file
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(blobName))
            {
                sw.Write("This is a test blob for uploading using user delegated SAS tokens");
            }

            BlobClient testingBlob = new BlobClient(blobUri.Uri);
            testingBlob.Upload(blobName);

            // Download the blob and print the content
            Console.WriteLine($"Reading content from the test blob {blobName}");
            Console.WriteLine();

            BlobDownloadInfo downloadInfo = testingBlob.DownloadContent();

            using (StreamReader sr = new StreamReader(downloadInfo.Content, true))
            {
                string line;
                while ((line = sr.ReadLine()) !=null)
                {
                    Console.WriteLine(line);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Finished reading content from test blob");
        }
    }
}
