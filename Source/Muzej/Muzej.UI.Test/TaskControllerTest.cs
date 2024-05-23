using NUnit.Framework;
using Moq;
using Muzej.Repository.Interfaces;
using Muzej.DomainObjects;
using Microsoft.AspNetCore.Mvc;
using Muzej.UI.Controllers;

namespace Muzej.UI.Tests.Controllers
{
    [TestFixture]
    public class TaskControllerTests
    {
        private Mock<IRepositoryWrapper> _mockRepo;
        private TaskController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _controller = new TaskController(null, _mockRepo.Object);
        }
        
        [Test]
        public void Create_ValidTask_ReturnsRedirectToActionResult()
        {
            // Arrange
            var newTask = new DomainObjects.Task { TaskId = 1, Description = "New Task" };
            _mockRepo.Setup(repo => repo.Tasks.CreateTask(newTask)).Returns(1);

            // Act
            var result = _controller.Create(newTask);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Create_InvalidTask_ReturnsBadRequestResult()
        {
            // Act
            var result = _controller.Create(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Create_CreationFails_ReturnsStatusCodeResult()
        {
            // Arrange
            var newTask = new DomainObjects.Task { TaskId = 1, Description = "New Task" };
            _mockRepo.Setup(repo => repo.Tasks.CreateTask(newTask)).Returns(-1);

            // Act
            var result = _controller.Create(newTask);
            
            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [Test]
        public void Delete_ExistingTask_ReturnsRedirectToActionResult()
        {
            // Arrange
            int taskId = 1;
            var task = new DomainObjects.Task { TaskId = taskId };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(taskId)).Returns(task);
            _mockRepo.Setup(repo => repo.Tasks.DeleteTask(taskId)).Returns(true);

            // Act
            var result = _controller.Delete(taskId);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Delete_NonExistingTask_ReturnsNotFoundResult()
        {
            // Arrange
            int taskId = 1;
            _mockRepo.Setup(repo => repo.Tasks.GetTask(taskId)).Returns((DomainObjects.Task)null);

            // Act
            var result = _controller.Delete(taskId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Delete_DeletionFails_ReturnsStatusCodeResult()
        {
            // Arrange
            int taskId = 1;
            var task = new DomainObjects.Task { TaskId = taskId };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(taskId)).Returns(task);
            _mockRepo.Setup(repo => repo.Tasks.DeleteTask(taskId)).Returns(false);

            // Act
            var result = _controller.Delete(taskId);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [Test]
        public void Update_ValidTask_ReturnsRedirectToActionResult()
        {
            // Arrange
            var task = new DomainObjects.Task { TaskId = 1, Description = "Task 1" };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(task.TaskId)).Returns(task);
            _mockRepo.Setup(repo => repo.Tasks.UpdateTask(task)).Returns(true);

            // Act
            var result = _controller.Update(task);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Update_InvalidTask_ReturnsBadRequestResult()
        {
            // Act
            var result = _controller.Update(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Update_NonExistingTask_ReturnsNotFoundResult()
        {
            // Arrange
            var task = new DomainObjects.Task { TaskId = 1, Description = "Task 1" };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(task.TaskId)).Returns((DomainObjects.Task)null);

            // Act
            var result = _controller.Update(task);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Update_UpdateFails_ReturnsStatusCodeResult()
        {
            // Arrange
            var task = new DomainObjects.Task { TaskId = 1, Description = "Task 1" };
            _mockRepo.Setup(repo => repo.Tasks.GetTask(task.TaskId)).Returns(task);
            _mockRepo.Setup(repo => repo.Tasks.UpdateTask(task)).Returns(false);

            // Act
            var result = _controller.Update(task);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
    }
}
