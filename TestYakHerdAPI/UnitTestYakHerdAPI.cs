using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YakHerd;
using YakHerdAPI.Controllers;

namespace TestYakHerdAPI
{
    [TestClass]
    public class UnitTestYakHerdAPI
    {
        private static YakHerdController _controller;

        private static Herd InitialHerd()
        {
            var herd = new Herd
            {
                LabYaks = new LabYak[]
                {
                    new LabYak { Name = "Betty-1", Age = (decimal)4, Sex = "f" },
                    new LabYak { Name = "Betty-2", Age = (decimal)8, Sex = "f" },
                    new LabYak { Name = "Betty-3", Age = (decimal)9.5, Sex = "f" },
                }
            };

            return herd;
        }

        [TestMethod]
        public void TestGetHerdDay13()
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
        public void TestPostOrderDay13_Ok()
        {
            // Arrange
            _controller = new YakHerdController(InitialHerd());

            // Act
            var result = _controller.Order(13, new YakHerdController.OrderFormat { Customer = "Medvedev", Order = new YakHerdController.StockFormat { Milk = 1100, Skins = 3 } });
            ActionResult endresult = result.Result;

            //Assert 
            Assert.AreEqual(201, ((ObjectResult)endresult).StatusCode);

            Assert.AreEqual(1100, ((YakHerdController.StockFormat)((ObjectResult)result.Result).Value).Milk);
            Assert.AreEqual(3, ((YakHerdController.StockFormat)((ObjectResult)result.Result).Value).Skins);
        }

        [TestMethod]
        public void TestPostOrderDay13_NotFound()
        {
            // Arrange
            _controller = new YakHerdController(InitialHerd());

            // Act
            var result = _controller.Order(13, new YakHerdController.OrderFormat { Customer = "Medvedev", Order = new YakHerdController.StockFormat { Milk = 11000, Skins = 30 } });

            Assert.AreEqual(404, ((StatusCodeResult)result.Result).StatusCode);
        }

        [TestMethod]
        public void TestPostOrderDay13_PartlyMilk()
        {
            // Arrange
            _controller = new YakHerdController(InitialHerd());

            // Act
            var result = _controller.Order(13, new YakHerdController.OrderFormat { Customer = "Medvedev", Order = new YakHerdController.StockFormat { Milk = 1100, Skins = 30 } });
            ActionResult endresult = result.Result;

            //Assert 
            Assert.AreEqual(206, ((ObjectResult)endresult).StatusCode);

            Assert.AreEqual(1100, ((YakHerdController.StockFormat)((ObjectResult)result.Result).Value).Milk);
            Assert.AreEqual(0, ((YakHerdController.StockFormat)((ObjectResult)result.Result).Value).Skins);
        }

        [TestMethod]
        public void TestPostOrderDay13_PartlySkins()
        {
            // Arrange
            _controller = new YakHerdController(InitialHerd());

            // Act
            var result = _controller.Order(13, new YakHerdController.OrderFormat { Customer = "Medvedev", Order = new YakHerdController.StockFormat { Milk = 11000, Skins = 3 } });
            ActionResult endresult = result.Result;

            //Assert 
            Assert.AreEqual(206, ((ObjectResult)endresult).StatusCode);

            Assert.AreEqual(3, ((YakHerdController.StockFormat)((ObjectResult)result.Result).Value).Skins);
            Assert.AreEqual(0, ((YakHerdController.StockFormat)((ObjectResult)result.Result).Value).Milk);
        }
    }
}
