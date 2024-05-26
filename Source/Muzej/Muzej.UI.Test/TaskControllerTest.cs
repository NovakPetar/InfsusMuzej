using NUnit.Framework;
using Moq;
using Muzej.Repository.Interfaces;
using Muzej.DomainObjects;
using Microsoft.AspNetCore.Mvc;
using Muzej.UI.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Muzej.UI.Tests.Controllers
{
    [TestFixture]
    public class TaskControllerTests
    {
        private Mock<IRepositoryWrapper> _mockRepo;
        private Mock<ILogger<TaskController>> _mockLogger;
        private TaskController _controller;
        private Mock<ISession> _mockSession;
        private DefaultHttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<TaskController>>();
            _controller = new TaskController(_mockLogger.Object, _mockRepo.Object);

            // Mock session
            _mockSession = new Mock<ISession>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var sessionValues = new Dictionary<string, byte[]>();

            _mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                        .Callback<string, byte[]>((key, value) => sessionValues[key] = value);

            _mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
                        .Returns((string key, out byte[] value) =>
                        {
                            return sessionValues.TryGetValue(key, out value);
                        });

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = _mockSession.Object;
            _controller.ControllerContext.HttpContext = _httpContext;
        }

        [Test]
        public void Create_ValidTask_ReturnsRedirectToActionResult()
        {
            var newTask = new DomainObjects.Task { TaskId = 1, Description = "New Task" };
            _mockRepo.Setup(repo => repo.Tasks.CreateTask(newTask)).Returns(1);

            var result = _controller.Create(newTask);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Create_InvalidTask_ReturnsBadRequestResult()
        {
            var result = _controller.Create(null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Create_CreationFails_ReturnsStatusCodeResult()
        {
            var newTask = new DomainObjects.Task { TaskId = 1, Description = "New Task" };
            _mockRepo.Setup(repo => repo.Tasks.CreateTask(newTask)).Returns(-1);

            IActionResult result = _controller.Create(newTask);

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Internal server error", objectResult.Value);
        }

        [Test]
        public void Delete_ExistingTask_ReturnsRedirectToActionResult()
        {
            int taskId = 1;
            int employeeId = 1;
            var task = new DomainObjects.Task { TaskId = taskId, EmployeeId = employeeId };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(taskId)).Returns(task);
            _mockRepo.Setup(repo => repo.Tasks.DeleteTask(taskId)).Returns(task);

            _httpContext.Session.SetString("Role", "TimetableManager");

            var result = _controller.Delete(taskId, employeeId);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Delete_NonExistingTask_ReturnsNotFoundResult()
        {
            int taskId = 1;
            int employeeId = 1;
            _mockRepo.Setup(repo => repo.Tasks.GetTask(taskId)).Returns((DomainObjects.Task)null);

            _httpContext.Session.SetString("Role", "TimetableManager");

            var result = _controller.Delete(taskId, employeeId);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Delete_DeletionFails_ReturnsRedirectToActionResult()
        {
            int taskId = 1;
            int employeeId = 1;
            var task = new DomainObjects.Task { TaskId = taskId };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(taskId)).Returns(task);
            _mockRepo.Setup(repo => repo.Tasks.DeleteTask(taskId)).Returns(task);

            _httpContext.Session.SetString("Role", "TimetableManager");

            var result = _controller.Delete(taskId, employeeId);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Update_ValidTask_ReturnsRedirectToActionResult()
        {
            var task = new DomainObjects.Task { TaskId = 1, Description = "Task 1" };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(task.TaskId)).Returns(task);
            _mockRepo.Setup(repo => repo.Tasks.UpdateTask(task)).Returns(true);

            var result = _controller.Update(task);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Update_InvalidTask_ReturnsBadRequestResult()
        {
            var result = _controller.Update(null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Update_NonExistingTask_ReturnsNotFoundResult()
        {
            var task = new DomainObjects.Task { TaskId = 1, Description = "Task 1" };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(task.TaskId)).Returns((DomainObjects.Task)null);

            var result = _controller.Update(task);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Update_UpdateFails_ReturnsStatusCodeResult()
        {
            var task = new DomainObjects.Task { TaskId = 1, Description = "Task 1" };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(task.TaskId)).Returns(task);
            _mockRepo.Setup(repo => repo.Tasks.UpdateTask(task)).Returns(false);

            var result = _controller.Update(task);

            var statusCodeResult = result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
    }
}
