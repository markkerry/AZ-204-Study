using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System;

namespace stgQueueReceive
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

            // Get the attributes of the queue
            stgQueue.FetchAttributes();

            // Get the count of the messages in the queue
            int? count = stgQueue.ApproximateMessageCount;

            for (int i = 0; i < count; i++)
            {
                // Get the message
                CloudQueueMessage stgMessage = stgQueue.GetMessage();
                // Display the message
                Console.WriteLine(stgMessage.AsString);
                // Delete the message
                stgQueue.DeleteMessage(stgMessage);
            }
            Console.WriteLine("Received messages for queue complete");
            Console.ReadLine();
        }
    }
}
