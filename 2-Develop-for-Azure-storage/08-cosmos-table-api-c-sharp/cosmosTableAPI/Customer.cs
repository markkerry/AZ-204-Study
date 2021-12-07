using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace cosmosTableAPI
{
    public class Customer : TableEntity
    {
        public string customername { get; set; }

        public Customer()
        {

        }
        public Customer(string city,string id,string name)
        {
            PartitionKey = city;
            RowKey = id;
            customername = name;
        }
    }
}