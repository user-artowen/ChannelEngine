using System;
using FakeItEasy;
using Xunit;
using System.Linq;
using ChannelEngineCommonData.Service;
using ChannelEngineCommonData.Model;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ChannelEngineDemoMVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ChannelEngineDemoTest
{
    public class OrderServiceTest
    {

        [Fact]
        public async void OrderService_GetAPI_Result_Test()
        {
            // Arrange        
            var orderService = A.Fake<IOrderService>();
            var orderList = A.CollectionOfDummy<Order>(100).ToList();
            A.CallTo(() => orderService.GetOrders()).Returns(orderList);

            // Act
            var orders = await orderService.GetOrders();

            // Assert
            Assert.True(orders.Count > 0);
        }



        [Fact]
        public async void OrderService_GetProductTop_Test()
        {
            // Arrange
            int topX = 5;
            var orderService = A.Fake<IOrderService>();
            var productList = A.CollectionOfDummy<Product>(topX).ToList();
            A.CallTo(() => orderService.GetProductTop(topX)).Returns(productList);

            // Act
            var topProducts = await orderService.GetProductTop(topX);

            // Assert
            Assert.True(topProducts.Count > 0);
            Assert.True(topProducts.Count <= 5);
        }

        [Fact]
        public async void OrderService_ProductUpdateStock_Test()
        {
            // Arrange
            int newStockValue = 25;
            var orderService = A.Fake<IOrderService>();
            var product = new Product();            

            // Act
            var isUpdated = await orderService.UpdateProductStock(product, newStockValue);

            // Assert
            Assert.False(isUpdated);            
        }
    }
}
