using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using YakHerd;

namespace TestYakHerd
{
    [TestClass]
    public class UnitTestYakHerd
    {

        private static Herd InitialHerd ()
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
        public void TestYakHerd13()
        {
            // Arrange
            var herd = InitialHerd();

            // Act
            herd.CalculateHerd(13);

            // Assert
            Assert.AreEqual((decimal)1104.48, herd.Milk);
            Assert.AreEqual(3, herd.Hides);
        }

        [TestMethod]
        public void TestYakHerd14()
        {
            // Arrange
            var herd = InitialHerd();

            // Act
            herd.CalculateHerd(14);

            // Assert
            Assert.AreEqual((decimal)1188.81, herd.Milk);
            Assert.AreEqual(4, herd.Hides);
        }

        [TestMethod]
        public void TestYakHerdWithMales()
        {
            // Arrange
            var herd = InitialHerd();
            herd.LabYaks = new List<LabYak>(herd.LabYaks) { new LabYak { Name = "Harry-1", Age = (decimal)5.5, Sex = "m" } }.ToArray();

            // Act
            herd.CalculateHerd(14);

            // Assert
            Assert.AreEqual((decimal)1188.81, herd.Milk);
            Assert.AreEqual(5, herd.Hides);
        }



}
}
