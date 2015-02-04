﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCAspNet.DataTables.Web;
using MVCAspNet.DataTables.Web.Controllers;

namespace MVCAspNet.DataTables.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DataTables()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.DataTables() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }
    }
}
