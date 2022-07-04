
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ChannelEngineCommonData.Model
{
    public class Product
    {
        public string Status { get; set; }
        public string IsFulfillmentByMarketplace { get; set; }
        [JsonProperty(PropertyName = "Gtin")]
        public string GTIN { get; set; }
        public string Description { get; set; }
        public OrderProductStock StockLocation { get; set; }
        public string ChannelProductNo { get; set; }
        public string MerchantProductNo { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateProductStockRequest
    {
        public string MerchantProductNo { get; set; }        
        public List<StockLocation> StockLocations { get; set; }
    }

    public class UpdateProductStockResponse
    {
        public int StatusCode { get; set; }
    }

    public class StockLocation
    {
        public int Stock { get; set; }
        public string StockLocationId { get; set; }
    }

}
