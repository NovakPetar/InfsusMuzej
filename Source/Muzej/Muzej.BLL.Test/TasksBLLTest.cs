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
        private Mock<IEmployeesRepository> _mockEmployeesRepository;

        [SetUp]
        public void Setup()
        {
            _mockTasksRepository = new Mock<ITasksRepository>();
            _mockEmployeesRepository = new Mock<IEmployeesRepository>();
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            _mockRepositoryWrapper.Setup(r => r.Tasks).Returns(_mockTasksRepository.Object);
            _mockRepositoryWrapper.Setup(r => r.Employees).Returns(_mockEmployeesRepository.Object);

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
            var employee = new DomainObjects.Employee // Mocking an employee object
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                JobId = 101 // Just an example job ID
            };
            _mockEmployeesRepository.Setup(r => r.GetEmployee(It.IsAny<int>())).Returns(employee);

            var result = _tasksBLL.UpdateTask(task);
            
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateTask_ShouldReturnTaskId_WhenTaskIsCreated()
        {
            var task = new DomainObjects.Task { TaskId = 1, EmployeeId = 1 }; // Assigning an employee ID
            var expectedTaskId = 1;
            _mockTasksRepository.Setup(r => r.CreateTask(task)).Returns(expectedTaskId);

            var employee = new DomainObjects.Employee // Mocking an employee object
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                JobId = 101 // Just an example job ID
            };
            _mockEmployeesRepository.Setup(r => r.GetEmployee(It.IsAny<int>())).Returns(employee);

            var result = _tasksBLL.CreateTask(task);

            Assert.AreEqual(expectedTaskId, result);
        }

        [Test]
        public void DeleteTask_ShouldReturnTrue_WhenTaskIsDeleted()
        {
            // Arrange
            var taskId = 1;
            var deletedTask = new DomainObjects.Task
            {
                TaskId = taskId,
                StartDateTime = DateTime.Now.AddHours(1), // Assuming task start time is set in the future
                EmployeeId = 1 // Assuming a valid employee ID
            };
            var employee = new DomainObjects.Employee // Mocking an employee object
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                JobId = 101 // Just an example job ID
            };
            _mockTasksRepository.Setup(r => r.DeleteTask(taskId)).Returns(deletedTask);
            _mockEmployeesRepository.Setup(r => r.GetEmployee(It.IsAny<int>())).Returns(employee);

            // Act
            var result = _tasksBLL.DeleteTask(taskId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
