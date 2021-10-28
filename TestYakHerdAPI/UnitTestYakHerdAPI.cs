using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YakHerd;
using YakHerdAPI.Controllers;

namespace TestYakHerdAPI
{
    [TestClass]
    public class UnitTestYakHerdAPI
    {
        public static YakHerdController _controller;

        private static Herd InitialHerd()
        {
            var herd = new Herd();
            herd.LabYaks = new LabYak[]
            {
                new LabYak { Name = "Betty-1", Age = (decimal)4, Sex = "f" },
                new LabYak { Name = "Betty-2", Age = (decimal)8, Sex = "f" },
                new LabYak { Name = "Betty-3", Age = (decimal)9.5, Sex = "f" },
            };

            return herd;
        }

        [TestMethod]
        public void TestGetHerd_13()
        {
            // Arrange
            _controller = new YakHerdController(InitialHerd());

            // Act
            var result = _controller.Herd(13);

            //Assert 
            Assert.AreEqual(3, result.Herd.Count);
            Assert.AreEqual(4, result.Herd[0].AgeLastShaved);
            Assert.AreEqual("Betty-1", result.Herd[0].Name);
        }

        [TestMethod]
        public void TestPostOrder_13()
        {
            // Arrange
            _controller = new YakHerdController(InitialHerd());

            // Act
            var result = _controller.Order(13, new YakHerdController.OrderFormat { Customer = "Medvedev", Order = new YakHerdController.StockFormat { Milk = 1100, Skins = 3 } });
            ActionResult endresult = result.Result;

            Assert.AreEqual(201, ((ObjectResult)endresult).StatusCode);
            
            //Assert 
            Assert.AreEqual(1100, ((YakHerdController.StockFormat)((ObjectResult)result.Result).Value).Milk);
            Assert.AreEqual(3, ((YakHerdController.StockFormat)((ObjectResult)result.Result).Value).Skins);
        }

        // TODO: assert null on milk




    }
}
