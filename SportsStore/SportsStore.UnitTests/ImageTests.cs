using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            Product prod = new Product { ProductID = 1, Name = "Product1", ImageData = new byte[] { },ImageMimeType="img/png" };
            Mock <IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { prod,
                                                              new Product {ProductID=2,Name="Product2" },
                                                              new Product {ProductID=3,Name="Product3" }});
            ProductController controller = new ProductController(mock.Object);
            ActionResult result=controller.GetImage(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(((FileResult)(result)).ContentType, prod.ImageMimeType);

        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data()
        {
            Product prod = new Product { ProductID = 1, Name = "Product1", ImageData = new byte[] { }, ImageMimeType = "img/png" };
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { prod,
                                                              new Product {ProductID=2,Name="Product2" },
                                                              new Product {ProductID=3,Name="Product3" }});
            ProductController controller = new ProductController(mock.Object);
            ActionResult result = controller.GetImage(5);

            Assert.IsNull(result);

        }
    }
}
