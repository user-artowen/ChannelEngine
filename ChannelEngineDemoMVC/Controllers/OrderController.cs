using ChannelEngineDemoMVC.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ChannelEngineCommonData.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using ChannelEngineCommonData.Model;

namespace ChannelEngineDemoMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        public async Task<ActionResult> ProductTop()
        {
            try
            {
                // get top 5 products
                var topProductList = await _orderService.GetProductTop(5);

                // pick a product to update stock to 25
                if (topProductList.Count > 0)
                {
                    var productToUpdate = topProductList[new Random().Next(topProductList.Count)];
                    await ProductUpdateStock(productToUpdate, 25);
                }

                return View(topProductList);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Order", new { message = e.Message });
            }
        }

        public async Task ProductUpdateStock(Product product, int newStockCnt)
        {
            // update product stock
            if (await _orderService.UpdateProductStock(product, newStockCnt))
                ViewBag.ProductUpdate = $"Product {product.MerchantProductNo} stock updated to {newStockCnt.ToString()}.";
            else
                ViewBag.ProductUpdate = $"Product {product.MerchantProductNo} stock update failed.";
        }

        public ActionResult Error(string message)
        {            
            return View("Error", new ErrorViewModel { Message = message });
        }
    }
}
