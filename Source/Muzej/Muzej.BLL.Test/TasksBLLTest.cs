using NUnit.Framework;
using Moq;
using Muzej.BLL;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using System.Collections.Generic;

namespace Muzej.BLL.Tests
{
    [TestFixture]
    public class TasksBLLTest
    {
        private TasksBLL _tasksBLL;
        private Mock<ITasksRepository> _mockTasksRepository;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        [SetUp]
        public void Setup()
        {
            _mockTasksRepository = new Mock<ITasksRepository>();
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            
            _mockRepositoryWrapper.Setup(r => r.Tasks).Returns(_mockTasksRepository.Object);
            
            _tasksBLL = new TasksBLL(_mockRepositoryWrapper.Object);
        }

        [Test]
        public void GetTask_ShouldReturnTask_WhenTaskExists()
        {
            var taskId = 1;
            var expectedTask = new DomainObjects.Task { TaskId= taskId };
            _mockTasksRepository.Setup(r => r.GetTask(taskId)).Returns(expectedTask);
            
            var result = _tasksBLL.GetTask(taskId);
            
            Assert.AreEqual(expectedTask, result);
        }

        [Test]
        public void GetTasksForEmployee_ShouldReturnTasks_WhenTasksExist()
        {
            var employeeId = 1;
            var expectedTasks = new List<DomainObjects.Task> { new DomainObjects.Task { TaskId = 1 }, new DomainObjects.Task { TaskId = 2 } };
            _mockTasksRepository.Setup(r => r.GetTasksForEmployee(employeeId)).Returns(expectedTasks);

            var result = _tasksBLL.GetTasksForEmployee(employeeId);
            
            Assert.AreEqual(expectedTasks, result);
        }

        [Test]
        public void UpdateTask_ShouldReturnTrue_WhenTaskIsUpdated()
        {
            var task = new DomainObjects.Task { TaskId = 1 };
            _mockTasksRepository.Setup(r => r.UpdateTask(task)).Returns(true);
            
            var result = _tasksBLL.UpdateTask(task);
            
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateTask_ShouldReturnTaskId_WhenTaskIsCreated()
        {
            var task = new DomainObjects.Task { TaskId = 1 };
            var expectedTaskId = 1;
            _mockTasksRepository.Setup(r => r.CreateTask(task)).Returns(expectedTaskId);
            
            var result = _tasksBLL.CreateTask(task);
            
            Assert.AreEqual(expectedTaskId, result);
        }

        [Test]
        public void DeleteTask_ShouldReturnTrue_WhenTaskIsDeleted()
        {
            var taskId = 1;
            //_mockTasksRepository.Setup(r => r.DeleteTask(taskId)).Returns(true);
            
            var result = _tasksBLL.DeleteTask(taskId);
            
            Assert.IsTrue(result);
        }
    }
}
