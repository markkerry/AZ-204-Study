using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;

namespace blobPropertiesAndMetadata
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Getting system properties demo");

            AppSettings appSettings = appSettings.LoadAppSettings();

            // Create a CloudBlobClient for working with the Storage Account
            CloudBlobClient blobClient = Common.CreateBlobClientStorageFromSAS(appSettings.SASToken, appSettings.AccountName);

            // Get a container reference for the new container
            CloudBlobContainer container = blobClient.GetContainerReference(appSettings.ContainerName);

            // Create the container if not already exists
            container.CreateIfNotExists();

            // Get container properties before getting their values
            container.FetchAttributes();
            Console.WriteLine($"Properties for container {container.StorageUri.PrimaryUri.ToString()}");
            System.Console.WriteLine($"ETag: {container.Properties.ETag}");
            System.Console.WriteLine($"LastModifiedUTC: {container.Properties.LastModified.ToString()}");
            System.Console.WriteLine($"Lease status: {container.Properties.LeaseStatus.ToString()}");
            System.Console.WriteLine();

            // Add some metadata to the container
            container.Metadata.Add("department", "Technical");
            container.Metadata["category"] = "Koowledge Base";
            container.Metadata.Add("docType", "pdfDocuments");

            // Save the containers metadata in Azure
            container.SetMetadata();

            // List the newly added metadata
            container.FetchAttributes();
            System.Console.WriteLine("Container's metadata:");
            foreach (var item in container.Metadata)
            {
                System.Console.Write($"\tKey: {item.Key}\t");
                System.Console.WriteLine($"\tValue: {item.Value}");
            }
        }
    }
}
