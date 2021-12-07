using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace blobPropertiesAndMetadata
{
    class Program
    {
        static string connectionString = "";
        static string containerName = "images";
        static string fileName = "sample.html";
        static void Main(string[] args)
        {
            GetProperties();
            GetMetadata();
            SetMetadata("tier", "1").Wait();
            Console.WriteLine("Operation complete");
            Console.ReadKey();
        }

        static void GetProperties()
        {
            // Create a connection to the SA, get an existing container, get an existing blob
            BlobServiceClient w_blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient w_containerClient = w_blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient w_blob = w_containerClient.GetBlobClient(fileName);

            // Get the Blob properties
            BlobProperties w_properties = w_blob.GetProperties();

            Console.WriteLine("The Content Type is {0}", w_properties.ContentType);
            Console.WriteLine("The Content Length is {0}", w_properties.ContentLength);
        }

        static void GetMetadata()
        {
            // Create a connection to the SA, get an existing container, get an existing blob
            BlobServiceClient w_blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient w_containerClient = w_blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient w_blob = w_containerClient.GetBlobClient(fileName);

            // Get the Blob properties
            BlobProperties w_properties = w_blob.GetProperties();

            // Iterate through all the properties
            foreach (var w_metadata in w_properties.Metadata)
            {
                Console.WriteLine(w_metadata.Key.ToString());
                Console.WriteLine(w_metadata.Value.ToString());
            }
        }

        static async Task SetMetadata(string w_key, string w_value)
        {
            // Create a connection to the SA, get an existing container, get an existing blob
            BlobServiceClient w_blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient w_containerClient = w_blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient w_blob = w_containerClient.GetBlobClient(fileName);

            // Get the Blob properties
            BlobProperties w_properties = w_blob.GetProperties();

            // Set the new metadata property. Create an IDictionary object with the required values
            IDictionary<string, string> w_metadata = new Dictionary<string, string>();
            w_metadata.Add(w_key, w_value);

            // Set the metadata properties
            await w_blob.SetMetadataAsync(w_metadata);
        }
    }
}
