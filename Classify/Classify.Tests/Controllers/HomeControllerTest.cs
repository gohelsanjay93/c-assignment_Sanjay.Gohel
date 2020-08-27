using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classify;
using Classify.Controllers;

namespace Classify.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();
            controller.Session["UserId"] = "Temp";
            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit()
        {            
            // Arrange
            HomeController controller = new HomeController();
            // Act
            controller.Session["UserId"] = "Temp";
            ViewResult result = controller.Edit(1) as ViewResult;
            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Create()
        {
            
            // Arrange
            HomeController controller = new HomeController();

            controller.Session["UserId"] = "Temp";
            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Login()
        {
            
            // Arrange
            UserController controller = new UserController();
           
            // Act
            ViewResult result = controller.Login() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Register()
        {

            // Arrange
            UserController controller = new UserController();

            // Act
            ViewResult result = controller.Register() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void NonActionmethods()
        {

            // Arrange
            UserController controller = new UserController();

            // Act
            ViewResult result = controller.MailHasbeebSent() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
