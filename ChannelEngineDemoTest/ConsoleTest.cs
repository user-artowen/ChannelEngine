using Xunit;
using System.Linq;
using FakeItEasy;
using ChannelEngineCommonData.Service;
using ChannelEngineCommonData.Model;
using ChannelEngineDemoConsole.Classes;

namespace ChannelEngineDemoTest
{
    public class ConsoleTest
    {

        [Fact]
        public async void ConsoleTest_Run_Test()
        {
            // Arrange
            int topX = 5;
            var orderService = A.Fake<IOrderService>();
            var productList = A.CollectionOfDummy<Product>(topX).ToList();
            A.CallTo(() => orderService.GetProductTop(topX)).Returns(productList);

            // Act
            var result = await ConsoleHelper.DisplayProductTop(orderService, topX);            

            // Assert
            Assert.True(result);
        }

    }
}
