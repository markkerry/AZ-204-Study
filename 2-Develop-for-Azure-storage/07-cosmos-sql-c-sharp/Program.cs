using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;


namespace cosmosDotNet
{
    class Program
    {
        static string databaseId = "";
        static string containerId = "";
        static string endpoint = "";
        static string accountkeys = "";

        static async Task Main(string[] args)
        {
            // Methods go here
            ReadItems().Wait();
            CreateItem().Wait();
            ReplaceItem().Wait();
            DeleteItem().Wait();
            Console.WriteLine("Task complete");
            Console.ReadKey();
        }

        private static async Task ReadItems()
        {
            // Create a connection to the Cosmos DB account using the endpoint and account keys
            using (CosmosClient client = new CosmosClient(endpoint, accountkeys))
            {
                // Get a handle to the database
                Database db = client.GetDatabase(databaseId);
                // Then to the container
                Container container = db.GetContainer(containerId);

                // Formulate the query
                ItemResponse<Customer> response = await container.ReadItemAsync<Customer>(
                    partitionKey: new PartitionKey("London"),
                    id: "1");

                // Get the request units charged
                Console.WriteLine("Request units consumed is {0}", response.RequestCharge);

                // Display the properties of the object
                Customer obj = (Customer)response;
                Console.WriteLine("The customer id is {0}", obj.Id);
                Console.WriteLine("The customer name is {0}", obj.Customername);
                Console.WriteLine("The customer city is {0}", obj.Customercity);
            }
        }

        private static async Task CreateItem()
        {
            Customer obj = new Customer()
            { Id = "4", Customername = "UserD", Customercity = "New York" };

            // Create a connection to the Cosmos DB account using the endpoint and account keys
            using (CosmosClient client = new CosmosClient(endpoint, accountkeys))
            {
                // Get a handle to the database
                Database db = client.GetDatabase(databaseId);
                // Then to the container
                Container container = db.GetContainer(containerId);

                // Create an item in the Cosmos DB Container
                ItemResponse<Customer> response = await container.CreateItemAsync(obj);

                // Get the request units charged
                Console.WriteLine("Request units consumed is {0}", response.RequestCharge);
            }
        }

        private static async Task ReplaceItem()
        {
            Customer obj = new Customer()
            { Id = "4", Customername = "UserE", Customercity = "New York" };

            // Create a connection to the Cosmos DB account using the endpoint and account keys
            using (CosmosClient client = new CosmosClient(endpoint, accountkeys))
            {
                // Get a handle to the database
                Database db = client.GetDatabase(databaseId);
                // Then to the container
                Container container = db.GetContainer(containerId);

                // Replace the item in the Cosmos DB Container
                ItemResponse<Customer> response = await container.ReplaceItemAsync(id: obj.Id, item: obj);

                // Get the request units charged
                Console.WriteLine("Request units consumed is {0}", response.RequestCharge);
            }
        }

        private static async Task DeleteItem()
        {
            // Create a connection to the Cosmos DB account using the endpoint and account keys
            using (CosmosClient client = new CosmosClient(endpoint, accountkeys))
            {
                // Get a handle to the database
                Database db = client.GetDatabase(databaseId);
                // Then to the container
                Container container = db.GetContainer(containerId);

                // Delete the item from the Cosmos DB Container
                ItemResponse<Customer> response = await container.DeleteItemAsync<Customer>(
                    partitionKey: new PartitionKey("New York"),
                    id: "4");

                // Get the request units charged
                Console.WriteLine("Request units consumed is {0}", response.RequestCharge);
            }
        }
    }
}       
