using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            AccountController controller = new AccountController(mock.Object);
            LoginViewModel model = new LoginViewModel { Username = "admin", Password = "secret" };
            ActionResult result=controller.Login(model, "/Url");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "/Url");

        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("user", "pass")).Returns(false);
            AccountController controller = new AccountController(mock.Object);
            LoginViewModel model = new LoginViewModel { Username = "user", Password = "pass" };

            ActionResult result = controller.Login(model, "/Url");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);

        }
    }
}
