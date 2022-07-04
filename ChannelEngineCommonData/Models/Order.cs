using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChannelEngineCommonData.Model
{
 
    public class OrdersResponse
    {
        [JsonProperty(PropertyName = "Content")]
        public List<Order> OrderRows { get; set; }
        public int? Count { get; set; }
        public int? TotalCount { get; set; }        
        public int? StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object ValidationErrors { get; set; }
    }

    public class Order
    {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Lines")]
        public List<Product> ProductList { get; set; }       
    }

    public class OrderProductStock
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }    
}