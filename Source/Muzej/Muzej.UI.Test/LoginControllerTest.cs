using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Muzej.UI.Controllers;
using NUnit.Framework;

namespace Muzej.UI.Tests.Controllers
{
    [TestFixture]
    public class LoginControllerTests
    {
        private LoginController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new LoginController();
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Login_Post_RedirectsToHomeIndex()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new Mock<ISession>();
            mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

            string role = "Admin";

            // Act
            var result = _controller.Login(role) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }
    }
}