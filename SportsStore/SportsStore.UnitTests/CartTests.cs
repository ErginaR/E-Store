using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Collections.Generic;
using System.Collections;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Product p1 = new Product { ProductID = 1, Price = 20 };
            Product p2 = new Product { ProductID = 2, Price = 12 };
            Cart cart = new Cart();

            cart.AddCart(p1, 2);
            cart.AddCart(p2, 3);

            CartLine[] results = cart.Lines.ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product,p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product { ProductID = 1, Price = 23 };
            Product p2 = new Product { ProductID = 2, Price = 22 };
            Cart cart = new Cart();

            cart.AddCart(p1,1);
            cart.AddCart(p2, 3);
            cart.AddCart(p1, 3);

            CartLine[] results = cart.Lines.OrderBy(p=>p.Product.ProductID).ToArray();

            Assert.AreEqual(results[0].Quantity, 4);
            Assert.AreEqual(results.Length, 2);

        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            Product p1 = new Product { ProductID = 1, Name = "Product1" };
            Product p2 = new Product { ProductID = 2, Name = "Product2" };
            Product p3 = new Product { ProductID = 3, Name = "Product3" };
            Cart cart = new Cart();

            cart.AddCart(p1, 2);
            cart.AddCart(p2, 4);
            cart.AddCart(p1, 4);
            cart.RemoveLine(p1);

            CartLine[] results = cart.Lines.ToArray();
            Assert.AreEqual(results.Length, 1);
            Assert.AreEqual(results[0].Product.ProductID, 2);
            Assert.AreEqual(cart.Lines.Where(c => c.Product == p1).Count(), 0);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Product p1 = new Product { ProductID = 1, Name = "Product1",Price=20M };
            Product p2 = new Product { ProductID = 2, Name = "Product2",Price=11M };
            Cart cart = new Cart();

            cart.AddCart(p1, 2);
            cart.AddCart(p2, 4);
            cart.AddCart(p1, 1);

            Assert.AreEqual(cart.ComputeTotalValue(), 104M);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product { ProductID = 1, Name = "Product1", Price = 20M };
            Product p2 = new Product { ProductID = 2, Name = "Product2", Price = 11M };
           
            Cart cart = new Cart();

            cart.AddCart(p1, 2);
            cart.AddCart(p2, 4);
            cart.AddCart(p1, 1);
            cart.Clear();

            Assert.AreEqual(cart.Lines.Count(), 0);

        }
        [TestMethod]
        public void Can_Add_to_Cart()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { new Product { ProductID=1,Name="Product1"},
                                                              new Product{ProductID=2,Name="Product2"}}.AsQueryable());
            CartController cartController = new CartController(mock.Object,null);
            Cart cart = new Cart();

            cartController.AddToCart(cart, 1, null);
            cartController.AddToCart(cart, 2, null);

            Assert.AreEqual(cart.Lines.Count(), 2);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID,1);

        }

        [TestMethod]
        public void Adding_Product_to_Cart_Goes_To_Cart_Screen()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { new Product { ProductID=1,Name="Product1"}}.AsQueryable());
            CartController cartController = new CartController(mock.Object,null);
            Cart cart = new Cart();

            RedirectToRouteResult target= cartController.AddToCart(cart, 1, "Url1");

            Assert.AreEqual(target.RouteValues["action"],"Index");
            Assert.AreEqual(target.RouteValues["returnUrl"],"Url1");

        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController cartController = new CartController(null,null);
            CartIndexViewModel cartIndex = (CartIndexViewModel)cartController.Index(cart, "Url1").ViewData.Model;

            Assert.AreEqual(cartIndex.ReturnUrl, "Url1");
            Assert.AreSame(cartIndex.Cart, cart);
        }
    }
}
