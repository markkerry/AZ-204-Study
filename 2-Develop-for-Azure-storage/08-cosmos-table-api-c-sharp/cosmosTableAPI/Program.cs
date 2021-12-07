using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace cosmosTableAPI
{
    class Program
    {
        static string connectionString = "";
        
        static void Main(string[] args)
        {
            GetItem().Wait();
            InsertItem().Wait();
            UpdateItem().Wait();
            DeleteItem().Wait();
            Console.WriteLine("Operation completed");
            Console.ReadKey();
        }

        static async Task GetItem()
        {
            string p_partitionKey = "Chicago";
            string p_rowKey = "1";

            // Create connection to Cosmos DB account
            CloudStorageAccount cosmosAccount = CloudStorageAccount.Parse(connectionString);
            // Create a table client
            CloudTableClient tableClient = cosmosAccount.CreateCloudTableClient();
            // Get a reference to an existing table
            CloudTable table = tableClient.GetTableReference("customer");
            // Retrieve an entity based on the partition and row key
            TableOperation operation = TableOperation.Retrieve<Customer>(p_partitionKey, p_rowKey);
            // Execute the operation
            TableResult result = await table.ExecuteAsync(operation);

            // Display properties of the returned object
            Customer obj = result.Result as Customer;
            if (obj != null)
            {
                Console.WriteLine("The customer id is {0}", obj.RowKey);
                Console.WriteLine("The customer name id {0}", obj.customername);
                Console.WriteLine("The customer city is {0}", obj.PartitionKey);
            }
        }

        static async Task InsertItem()
        {
            // Add the new customer object
            Customer obj = new Customer("Miami", "4", "UserD");

             // Create connection to Cosmos DB account
            CloudStorageAccount cosmosAccount = CloudStorageAccount.Parse(connectionString);
            // Create a table client
            CloudTableClient tableClient = cosmosAccount.CreateCloudTableClient();
            // Get a reference to an existing table
            CloudTable table = tableClient.GetTableReference("customer");
            // Insert an entity
            TableOperation operation = TableOperation.Insert(obj);
            // Execute the operation
            TableResult result = await table.ExecuteAsync(operation);
        }

        static async Task UpdateItem()
        {
            // Add the new customer object
            Customer obj = new Customer("Miami", "4", "UserD");

             // Create connection to Cosmos DB account
            CloudStorageAccount cosmosAccount = CloudStorageAccount.Parse(connectionString);
            // Create a table client
            CloudTableClient tableClient = cosmosAccount.CreateCloudTableClient();
            // Get a reference to an existing table
            CloudTable table = tableClient.GetTableReference("customer");
            // Update or replace an entity
            TableOperation operation = TableOperation.InsertOrReplace(obj);
            // Execute the operation
            TableResult result = await table.ExecuteAsync(operation);
        }

        static async Task DeleteItem()
        {
            string p_partitionKey = "Chicago";
            string p_rowKey = "1";

            // Create connection to Cosmos DB account
            CloudStorageAccount cosmosAccount = CloudStorageAccount.Parse(connectionString);
            // Create a table client
            CloudTableClient tableClient = cosmosAccount.CreateCloudTableClient();
            // Get a reference to an existing table
            CloudTable table = tableClient.GetTableReference("customer");

            // Retrieve and existing item
            TableOperation operation = TableOperation.Retrieve<Customer>(p_partitionKey, p_rowKey);
            TableResult result = await table.ExecuteAsync(operation);
            Customer obj = result.Result as Customer;

            // Perform the delete operation
            TableOperation operation = TableOperation.Delete(obj);
            TableResult deleteResult = await table.ExecuteAsync(operation);
        }
    }
}
