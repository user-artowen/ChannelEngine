using ChannelEngineCommonData.Model;
using ChannelEngineCommonData.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelEngineDemoConsole.Classes
{
    public static class ConsoleHelper
    {
        public static async Task<bool> DisplayProductTop(IOrderService orderService, int topX)
        {
            var productList = await orderService.GetProductTop(topX);
            Console.WriteLine("Top 5 Products from Orders" + Environment.NewLine);            
            foreach (var product in productList)
            {
                Console.WriteLine(
                    $"{product.MerchantProductNo,20}" +
                    $"{product.GTIN,20}" +
                    $"{product.Quantity,10}" +
                    $"{product.Description,50}"
                    );
                    
            }

            // pick a product to update stock to 25
            if (productList.Count > 0)
            {
                Console.WriteLine("");
                int newStockCnt = 25;
                var productToUpdate = productList[new Random().Next(productList.Count)];
                if (await orderService.UpdateProductStock(productToUpdate, newStockCnt))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Product {productToUpdate.MerchantProductNo} stock updated to {newStockCnt.ToString()}");
                }
                else
                    Console.WriteLine($"Product {productToUpdate.MerchantProductNo} update stock failed.");
            }

            return true;
        }


    }
}
