using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System;

namespace stgQueueSend
{
    class Program
    {
        private static string stgConnectionString = "";
        static void Main(string[] args)
        {
            // Connect to the stroage account
            CloudStorageAccount stgAccount = CloudStorageAccount.Parse(stgConnectionString);
            // Create a queue client
            CloudQueueClient stgClient = stgAccount.CreateCloudQueueClient();
            // Create reference to client
            CloudQueue stgQueue = stgClient.GetQueueReference("az204-queue");

            for (int i = 1; i < 10; i++)
            {
                string message = $"This is message {i}";
                CloudQueueMessage stgMessage = new CloudQueueMessage(message);
                stgQueue.AddMessage(stgMessage);
                Console.WriteLine(message);
            }
            Console.WriteLine("Sent the message");
            Console.ReadLine();
        }
    }
}
