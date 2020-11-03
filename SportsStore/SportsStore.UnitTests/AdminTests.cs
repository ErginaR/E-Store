using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { new Product{ProductID=1,Name="Product1"},
                                                              new Product{ProductID=2,Name="Product3"},
                                                              new Product{ProductID=3,Name="Product3"} });
            AdminController controller = new AdminController(mock.Object);
            Product[] products=((IEnumerable<Product>)(controller.Index().ViewData.Model)).ToArray();

            Assert.AreEqual(products[0].ProductID, 1);
            Assert.AreEqual(products[1].ProductID, 2);
            Assert.AreEqual(products[2].ProductID, 3);

        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { new Product { ProductID=1,Name="Product1"},
                                                              new Product { ProductID=2,Name="Product2"},
                                                              new Product { ProductID=3,Name="Product3"}});
            AdminController controller = new AdminController(mock.Object);
            Product product=controller.Edit(2).ViewData.Model as Product;

            Assert.AreEqual(product.ProductID, 2);

        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { new Product { ProductID=1,Name="Product1"},
                                                              new Product { ProductID=2,Name="Product2"},
                                                              new Product { ProductID=3,Name="Product3"}});
            AdminController controller = new AdminController(mock.Object);
            Product product = controller.Edit(4).ViewData.Model as Product;

            Assert.IsNull(product);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController controller = new AdminController(mock.Object);
            Product product = new Product { ProductID = 1, Name = "Product1" };
            ActionResult result=controller.Edit(product);
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Cannot_Save_Valid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController controller = new AdminController(mock.Object);
            Product product = new Product { ProductID = 1, Name = "Product1" };
            controller.ModelState.AddModelError("error", "Ka nje error");
            ActionResult result = controller.Edit(product);
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()),Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            Product product = new Product { ProductID = 2, Name = "Product2" };

            mock.Setup(m => m.Products).Returns(new Product[] { new Product{ProductID=1,Name="Product1"},
                                                                product,
                                                                new Product{ProductID=3,Name="Product3"} });
            AdminController controller = new AdminController(mock.Object);
            controller.Delete(product.ProductID);
            mock.Verify(m => m.DeleteProduct(2));
        }


    }
}
