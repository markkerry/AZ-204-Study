using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace changeBlobAccessTier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Moving blob between access tiers");
            Task.Run(async () => await StartContainersDemo()).Wait();
        }

        public static async Task StartContainersDemo()
        {
            string BlobFileName = "logs.zip";
            AppSettings appSettings = AppSettings.LoadAppSettings();

            // Get a cloud client for the Storage Account
            BlobServiceClient blobClient = Common.CreateBlobClientStorageFromSAS(appSettings.SASConnectionString);

            // Get a reference for each container
            var containerReference = blobClient.GetBlobContainerClient(appSettings.ContainerName);

            // Get a reference for the blob
            var blobReference = containerReference.GetBlobClient(BlobFileName);

            // Get current access tier
            BlobProperties blobProperties = await blobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Access Tier: {blobProperties.AccessTier}\t" +
                $"Inferred: {blobProperties.AccessTierInferred}\t" +
                $"Date last Access Tier change: {blobProperties.AccessTierChangedOn}");
            
            // Changed access tier to cool
            blobReference.SetAccessTier(AccessTier.Cool);

            // Get current access tier
            blobProperties = await blobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Access Tier: {blobProperties.AccessTier}\t" +
                $"Inferred: {blobProperties.AccessTierInferred}\t" +
                $"Date last Access Tier change: {blobProperties.AccessTierChangedOn}");
            
            // Changed access tier to archive
            blobReference.SetAccessTier(AccessTier.Archive);

            // Get current access tier
            blobProperties = await blobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Access Tier: {blobProperties.AccessTier}\t" +
                $"Inferred: {blobProperties.AccessTierInferred}\t" +
                $"Date last Access Tier change: {blobProperties.AccessTierChangedOn}");
            
            // Changed access tier to hot
            blobReference.SetAccessTier(AccessTier.Hot);

            // Get current access tier
            blobProperties = await blobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Access Tier: {blobProperties.AccessTier}\t" +
                $"Inferred: {blobProperties.AccessTierInferred}\t" +
                $"Date last Access Tier change: {blobProperties.AccessTierChangedOn}\t" +
                $"Archive Status: {blobProperties.ArchiveStatus}");
        }
    }
}
