using NUnit.Framework;
using Moq;
using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using Muzej.SqlServerRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Task = Muzej.DAL.Models.Task;

namespace Muzej.SqlServerRepository.Tests
{
    [TestFixture]
    public class TasksRepositoryTests
    {
        private ITasksRepository _repository;
        private Mock<MUZContext> _mockContext;
        private List<Task> _tasks;

        [SetUp]
        public void Setup()
        {
            _tasks = new List<Task>
            {
                new Task { TaskId = 1, Description = "Task 1", EmployeeId = 1 },
                new Task { TaskId = 2, Description = "Task 2", EmployeeId = 1 }
            };

            _mockContext = new Mock<MUZContext>();

            var mockSet = CreateMockTaskDbSet(_tasks);

            _mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            _repository = new TasksRepository(_mockContext.Object);
        }

        [Test]
        public void GetTask_ExistingTask_ReturnsTask()
        {
            int existingTaskId = 1;

            var task = _repository.GetTask(existingTaskId);

            Assert.IsNotNull(task);
            Assert.AreEqual(existingTaskId, task.TaskId);
        }

        [Test]
        public void GetTask_NonExistingTask_ReturnsNull()
        {
            int nonExistingTaskId = 100;

            var task = _repository.GetTask(nonExistingTaskId);

            Assert.IsNull(task);
        }

        [Test]
        public void GetTasksForEmployee_ReturnsTasks()
        {
            int employeeId = 1;

            var tasks = _repository.GetTasksForEmployee(employeeId);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count);
        }

        [Test]
        public void UpdateTask_ExistingTask_UpdatesTask()
        {
            var task = _repository.GetTask(1);
            task.Description = "UpdatedTaskDescription";

            var result = _repository.UpdateTask(task);

            Assert.IsTrue(result);
            Assert.AreEqual("UpdatedTaskDescription", _tasks.First(e => e.TaskId == 1).Description);
        }

        [Test]
        public void CreateTask_NewTask_CreatesTask()
        {
            var newTask = new DomainObjects.Task
            {
                TaskId = 3,
                Description = "New Task",
                EmployeeId = 1
            };

            _repository.CreateTask(newTask);

            Assert.IsTrue(_tasks.Any(e => e.TaskId == newTask.TaskId));
        }

        [Test]
        public void DeleteTask_ExistingTask_DeletesTask()
        {
            int existingTaskId = 1;

            var deletedTask = _repository.DeleteTask(existingTaskId);

            Assert.IsNotNull(deletedTask);
            Assert.AreEqual(existingTaskId, deletedTask.TaskId);
            Assert.IsFalse(_tasks.Any(e => e.TaskId == existingTaskId));
        }


        public static Mock<DbSet<Task>> CreateMockTaskDbSet(List<Task> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<Task>>();
            mockSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Task>())).Callback<Task>(entity =>
            {
                data.Add(entity);
            });
            mockSet.Setup(m => m.Remove(It.IsAny<Task>())).Callback<Task>(entity => data.Remove(entity));
            return mockSet;
        }
    }
}
