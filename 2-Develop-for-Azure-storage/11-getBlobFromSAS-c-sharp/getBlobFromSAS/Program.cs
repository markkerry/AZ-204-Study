using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System;
using System.IO;
using System.Threading.Tasks;

namespace getBlobFromSAS
{
    class Program
    {
        static string accountname = "";
        static string accountkey = "";
        static string containerName = "images";
        static string blobname = "sample.html";

        static async Task Main(string[] args)
        {
            Uri SASUri = GetBlobSAS();
            Console.WriteLine(SASUri);
            ReadBlob(SASUri).Wait();
            Console.ReadKey();
        }

        static Uri GetBlobSAS()
        {
            // Generate the SAS
            BlobSasBuilder w_sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                BlobName = blobname,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1) // The SAS is only valid for an hour
            };

            // Specify the read permissions for the blob
            w_sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Create an object of StorageSharedKeyCredential using the account name and account key
            var storageSharedKeyCredential = new StorageSharedKeyCredential(accountname, accountkey);

            // Get the SAS token
            string w_sasToken = w_sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();

            // Build the full URI to the Blob SAS
            UriBuilder w_fullUri = new UriBuilder()
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.core.windows.net", accountname),
                Path = string.Format("{0}/{1}", containerName, blobname),
                Query = w_sasToken
            };

            return w_fullUri.Uri;
        }

        static async Task ReadBlob(Uri p_SASUri)
        {
            BlobClient w_blobClient = new BlobClient(p_SASUri, null);

            BlobDownloadInfo w_blobDownloadInfo = await w_blobClient.DownloadAsync();
            using (StreamReader reader = new StreamReader(w_blobDownloadInfo.Content, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
