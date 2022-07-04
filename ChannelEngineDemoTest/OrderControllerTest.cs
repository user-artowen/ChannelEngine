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
    public class OrderControllerTest
    {               

        [Fact]
        public async void ProductTop_View_Test()
        {
            // Arrange
            int topX = 5;
            var loggerService = A.Fake<ILogger<OrderController>>();
            var orderService = A.Fake<IOrderService>();
            var productTopList = A.CollectionOfDummy<Product>(100).ToList();                        
            A.CallTo(() => orderService.GetProductTop(topX)).Returns(productTopList);
            var controller = new OrderController(loggerService, orderService);

            // Act
            var viewResult = await controller.ProductTop() as ViewResult;
            
            // Assert
            Assert.True(viewResult.Model != null);
        }

        [Fact]
        public void Error_View_Test()
        {
            // Arrange            
            var loggerService = A.Fake<ILogger<OrderController>>();
            var orderService = A.Fake<IOrderService>();
            var controller = new OrderController(loggerService, orderService);

            // Act
            var viewResult = controller.Error("Mock Error") as ViewResult;

            // Assert
            Assert.True(viewResult.Model != null);
        }
        
    }
}
