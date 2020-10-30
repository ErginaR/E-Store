using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.Domain.Entities;
using System.Collections.Generic;
using SportsStore.WebUI.Controllers;
using System.Linq;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;
using System.Web.UI;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Pagination()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>(); 
            mock.Setup(p=>p.Products).Returns(new Product[]{ new Product(){ProductID=1,Name="Product1"},
                                                             new Product(){ProductID=2,Name="Product2"},
                                                             new Product(){ProductID=3,Name="Product3"},
                                                             new Product(){ProductID=4,Name="Product4"},               
                                                             new Product(){ProductID=5,Name="Product5"},
                                                             new Product(){ProductID=6,Name="Product6"},
                                                             new Product(){ProductID=7,Name="Product7"},
                                                             new Product(){ProductID=8,Name="Product8"}});

            ProductController controller = new ProductController(mock.Object);
            controller.Pagesize =4;
            ProductsListViewModel result=(ProductsListViewModel)controller.List(null,1).Model;//page=1
            Product[] products = result.Products.ToArray();
            Assert.AreEqual(products.Length, 4);
            Assert.IsTrue(products[0].ProductID == 1);
            Assert.IsTrue(products[1].ProductID==2);
            Assert.IsTrue(products[2].ProductID == 3);
            Assert.IsTrue(products[3].Name =="Product4");
        }

        [TestMethod]
        public void Can_Genereate_Page_Links()
        {
            HtmlHelper htmlHelper = null;
            PagingInfo pagingInfo = new PagingInfo{ TotalItems = 30, CurrentPage = 2, ItemsPerPage = 10 };
            Func<int, string> UrlMethod = i => "Page" + i;

            MvcHtmlString result=htmlHelper.PageLinks(pagingInfo, UrlMethod);
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a><a class=""btn btn-default btn-success selected"" href=""Page2"">2</a><a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Paination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new List<Product> { new Product { ProductID=1,Name="product1"},
                                                                  new Product { ProductID=2,Name="product2"},
                                                                  new Product { ProductID=3,Name="product3"},
                                                                  new Product { ProductID=4,Name="product4"},
                                                                  new Product { ProductID=5,Name="product5"},
                                                                  new Product { ProductID=6,Name="product6"},
                                                                });

            ProductController controller = new ProductController(mock.Object);
            controller.Pagesize = 4;
            ProductsListViewModel productsListView =(ProductsListViewModel)controller.List(null,2).Model;

            Assert.IsTrue(productsListView.PagingInfo.CurrentPage == 2);
            Assert.IsTrue(productsListView.PagingInfo.ItemsPerPage == 4);
            Assert.IsTrue(productsListView.PagingInfo.TotalPages == 2);
            Assert.IsTrue(productsListView.PagingInfo.TotalItems == 6);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { new Product { ProductID=1,Name="Product1",Category="Category3"},
                                                                new Product { ProductID=2,Name="Product2",Category="Category2"},
                                                                new Product { ProductID=3,Name="Product3",Category="Category1"},
                                                                new Product { ProductID=4,Name="Product4",Category="Category1"},
                                                                new Product { ProductID=5,Name="Product5",Category="Category3"},
                                                                new Product { ProductID=6,Name="Product6",Category="Category1"},
                                                             });
            ProductController controller = new ProductController(mock.Object);
            controller.Pagesize = 3;
            ProductsListViewModel productsListViewModel = (ProductsListViewModel)controller.List("Category1", 1).Model;
            Product[] products = productsListViewModel.Products.ToArray();

            Assert.IsTrue("Category1" == productsListViewModel.CurrentCategory);
            Assert.AreEqual(products.Length, 3);
            Assert.IsTrue(products[0].ProductID == 3 && products[0].Category=="Category1");
            Assert.IsTrue(products[1].ProductID == 4);
            Assert.IsTrue(products[2].ProductID == 6);

        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { new Product{ProductID=1,Name="Product1",Category="Soccer"},
                                                                new Product{ProductID=2,Name="Product2",Category = "Chess"},
                                                                new Product{ProductID=3,Name="Product3",Category = "Soccer"},
                                                                new Product{ProductID=4,Name="Product4",Category = "WaterSports"} });
            NavController controller = new NavController(mock.Object);

            string[] categories = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            Assert.AreEqual(categories.Length, 3);
            Assert.AreEqual(categories[0], "Chess");
            Assert.AreEqual(categories[1], "Soccer");
            Assert.AreEqual(categories[2], "WaterSports");
        }
        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] { new Product { ProductID=1,Category="Category1"},
                                                              new Product { ProductID=2,Category="Category2"},
                                                              new Product { ProductID=3,Category="Category1"},
                                                              new Product { ProductID=4,Category="Category1"},
                                                              new Product { ProductID=5,Category="Category1"}});
            ProductController controller = new ProductController(mock.Object);
            //controller.Pagesize = 3;
            PagingInfo info=((ProductsListViewModel)controller.List("Category1").Model).PagingInfo;
            PagingInfo info2 = ((ProductsListViewModel)controller.List("Category2").Model).PagingInfo;
            PagingInfo info3 = ((ProductsListViewModel)controller.List(null).Model).PagingInfo;

            Assert.IsTrue(info.TotalItems == 4);
            //Assert.IsTrue(info.TotalPages == 2);
            Assert.IsTrue(info2.TotalItems == 1);
            //Assert.IsTrue(info2.TotalPages == 1);
            Assert.IsTrue(info3.TotalItems == 5);
            //Assert.IsTrue(info3.TotalPages == 2);
        }

        
    }
}
