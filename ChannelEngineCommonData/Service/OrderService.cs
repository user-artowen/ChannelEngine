using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChannelEngineCommonData.Model;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ChannelEngineCommonData.Helper;
using System.Net.Http;

namespace ChannelEngineCommonData.Service
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
        Task<List<Product>> GetProductTop_FromOrders(int topX);
        Task<List<Product>> GetProductTop(int topX);
        Task<bool> UpdateProductStock(Product product, int newStockValue);
    }
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IConfiguration _configuration;

        private OrdersResponse ordersResponseData;
        public OrderService(ILogger<OrderService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<Order>> GetOrders()
        {
            try
            {
                if (ordersResponseData == null)
                {
                    _logger.LogDebug($"Call GetOrders API");
                    string uri = _configuration.GetSection("API-GetOrders").Value;
                    string content = await APIHelper.Request(uri, HttpMethod.Get, null, "", "content/json");
                    ordersResponseData = JsonConvert.DeserializeObject<OrdersResponse>(content);
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug($"GetAPI GetOrders error {e.Message}");
                ordersResponseData.Success = false;
            }

            if (!ordersResponseData.Success)
                throw new Exception("Call API failed.");
            else
                return ordersResponseData.OrderRows;
        }

        public async Task<List<Product>> GetProductTop_FromOrders(int topX)
        {
            Dictionary<string, int> cntIdx = new();
            List<Product> productList = new();


            // orders            
            foreach (Order o in await GetOrders())
                foreach (Product p in o.ProductList)
                    productList.Add(p);

            // count MerchantProductNo
            foreach (Product p in productList)
            {
                if (!cntIdx.ContainsKey(p.MerchantProductNo))
                    cntIdx.Add(p.MerchantProductNo, 0);
                cntIdx[p.MerchantProductNo] = cntIdx[p.MerchantProductNo] + p.Quantity;
            }

            // return Top 5 products
            List<Product> topXProducts = cntIdx.Select(c => new Product
            {
                MerchantProductNo = c.Key,
            }).
            Select(p =>
            {
                p.Quantity = cntIdx[p.MerchantProductNo];
                return p;
            }).
            OrderByDescending(p => p.Quantity).
            Take(topX).
            Select(p =>
            {
                p.Description = productList.FirstOrDefault(pl => pl.MerchantProductNo == p.MerchantProductNo).Description;
                p.GTIN = productList.FirstOrDefault(pl => pl.MerchantProductNo == p.MerchantProductNo).GTIN;
                p.StockLocation = productList.FirstOrDefault(pl => pl.MerchantProductNo == p.MerchantProductNo).StockLocation;
                return p;
            }).
            ToList();

            return topXProducts;
        }

        public async Task<List<Product>> GetProductTop(int topX)
        {
            _logger.LogDebug($"GetProductTop");
            List<Product> list = await GetProductTop_FromOrders(topX);
            return list;
        }

        public async Task<bool> UpdateProductStock(Product product, int newStockValue)
        {
            if (product == null | product.MerchantProductNo == string.Empty)
                return false;

            _logger.LogDebug($"Call UpdateProductStock API");
            string
                uri = _configuration.GetSection("API-UpdateStock").Value,
                body = JsonConvert.SerializeObject
                (
                  new List<UpdateProductStockRequest>() {
                      new UpdateProductStockRequest
                      {
                          MerchantProductNo = product.MerchantProductNo,
                          StockLocations =  new List<StockLocation> {
                              new StockLocation
                              {
                                  Stock = newStockValue,
                                  StockLocationId = product.StockLocation.Id
                              }
                          }
                      }
                  }
                  );
            string content = await APIHelper.Request(uri, HttpMethod.Put, null, body, "content/json");
            var response = JsonConvert.DeserializeObject<UpdateProductStockResponse>(content);

            if (response.StatusCode == 200)
            {
                _logger.LogDebug($"Call UpdateProductStock {product.MerchantProductNo} stock location {product.StockLocation.Id} value {newStockValue.ToString()}");
                return true;
            }
            else
                return false;
        }

    }

}
