using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace moveStorageBlob
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Copy items between Containers demo!");
            Task.Run(async () => await StartContainersDemo()).Wait();
            Console.WriteLine("Move items between Storage Accounts demo!");
            Task.Run(async () => await StartAccountDemo()).Wait();
        }

        public static async Task StartContainersDemo()
        {
            string sourceBlobFileName = "Logs.zip";
            AppSettings appSettings = AppSettings.LoadAppSettings();

            // Get a cloud client for the source Storage Account
            BlobServiceClient sourceClient = Common.CreateBlobClientStorageFromSAS(appSettings.SourceSASConnectionString);

            // Get a reference for each container
            var sourceContainerReference = sourceClient.GetBlobContainerClient(appSettings.SourceContainerName);
            var destinationContainerReference = sourceClient.GetBlobContainerClient(appSettings.DestinationContainerName);

            // Get a reference for the source blob
            var sourceBlobReference = sourceContainerReference.GetBlobClient(sourceBlobFileName);
            var destinationBlobReference = destinationContainerReference.GetBlobClient(sourceBlobFileName);

            // Get the lease status of the source blob
            BlobProperties sourceBlobProperties = await sourceBlobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Lease status: {sourceBlobProperties.LeaseStatus}" +
                $"\tstate: {sourceBlobProperties.LeaseState}" +
                $"\tduration: {sourceBlobProperties.LeaseDuration}");

            // Acquire an infinate lease. To set a timespan use Timespan.FromSeconds(seconds)
            // which should be between 15 and 60
            string leaseID = Guid.NewGuid().ToString();
            BlobLeaseClient sourceLease = sourceBlobReference.GetBlobLeaseClient(leaseID);

            System.Console.WriteLine("Acquiring a lease");
            sourceLease.Acquire(new TimeSpan(-1));

            sourceBlobProperties = await sourceBlobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Lease status: {sourceBlobProperties.LeaseStatus}" +
                $"\tstate: {sourceBlobProperties.LeaseState}" +
                $"\tduration: {sourceBlobProperties.LeaseDuration}");

            // Copy the blob from the source container to the destination container
            System.Console.WriteLine("Starting copy");
            await destinationBlobReference.StartCopyFromUriAsync(sourceBlobReference.Uri);

            // Release the lease aquired previously
            System.Console.WriteLine("Releasing the lease");
            sourceLease.Release();
        }

        public static async Task StartAccountDemo()
        {
            string sourceBlobFileName = "Logs.zip";
            AppSettings appSettings = AppSettings.LoadAppSettings();

            // Get a cloud client for the source and destination Storage Accounts
            BlobServiceClient sourceClient = Common.CreateBlobClientStorageFromSAS(appSettings.SourceSASConnectionString);
            BlobServiceClient destinationClient = Common.CreateBlobClientStorageFromSAS(appSettings.DestinationSASConnectionString);

            // Get a reference for each container
            var sourceContainerReference = sourceClient.GetBlobContainerClient(appSettings.SourceContainerName);
            var destinationContainerReference = destinationClient.GetBlobContainerClient(appSettings.DestinationContainerName);

            // Get a reference for the source blob
            var sourceBlobReference = sourceContainerReference.GetBlobClient(sourceBlobFileName);
            var destinationBlobReference = destinationContainerReference.GetBlobClient(sourceBlobFileName);

            // Move the blob from the source container to the destination container
            await destinationBlobReference.StartCopyFromUriAsync(sourceBlobReference.Uri);
            await sourceBlobReference.DeleteAsync();
        }
    }
}
