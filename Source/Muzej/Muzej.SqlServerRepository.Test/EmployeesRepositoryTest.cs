using NUnit.Framework;
using Moq;
using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using Muzej.SqlServerRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Task = Muzej.DAL.Models.Task;
namespace Muzej.SqlServerRepository.Tests
{
    [TestFixture]
    public class EmployeesRepositoryTests
    {
        private IEmployeesRepository _repository;
        private Mock<MUZContext> _mockContext;
        private Mock<DbSet<Employee>> _mockEmployeeSet;
        private Mock<DbSet<DAL.Models.Task>> _mockTaskSet;
        private List<Employee> _employees;
        private List<DAL.Models.Task> _tasks;

        [SetUp]
        public void Setup()
        {
            _employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
                new Employee { EmployeeId = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com" }
            };

            _tasks = new List<DAL.Models.Task>
            {
                new DAL.Models.Task { TaskId = 1, Description = "Task 1", EmployeeId = 1 },
                new DAL.Models.Task { TaskId = 2, Description = "Task 2", EmployeeId = 1 }
            };

            _mockContext = new Mock<MUZContext>();

            _mockEmployeeSet = CreateMockEmployeeDbSet(_employees);
            _mockTaskSet = CreateMockTaskDbSet(_tasks);

            _mockContext.Setup(m => m.Employees).Returns(_mockEmployeeSet.Object);
            _mockContext.Setup(m => m.Tasks).Returns(_mockTaskSet.Object);

            _repository = new EmployeesRepository(_mockContext.Object);
        }

        private Mock<DbSet<Employee>> CreateMockEmployeeDbSet(List<Employee> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<Employee>>();
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Muzej.DAL.Models.Employee>())).Callback<Muzej.DAL.Models.Employee>(entity =>
            {
                data.Add(entity);
                var entry = new Mock<EntityEntry<Muzej.DAL.Models.Employee>>();
                entry.Setup(e => e.Entity).Returns(entity);
                mockSet.Setup(m => m.Add(entity)).Returns(entry.Object);
            });
            mockSet.Setup(m => m.Remove(It.IsAny<Employee>())).Callback<Employee>(entity => data.Remove(entity));
            mockSet.Setup(m => m.Update(It.IsAny<Employee>())).Callback<Employee>(entity =>
            {
                var item = data.FirstOrDefault(i => i.EmployeeId == entity.EmployeeId);
                if (item != null)
                {
                    data.Remove(item);
                }
                data.Add(entity);
            });
            return mockSet;
        }

        private Mock<DbSet<DAL.Models.Task>> CreateMockTaskDbSet(List<DAL.Models.Task> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<DAL.Models.Task>>();
            mockSet.As<IQueryable<DAL.Models.Task>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<DAL.Models.Task>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<DAL.Models.Task>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<DAL.Models.Task>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<DAL.Models.Task>())).Callback<DAL.Models.Task>(entity =>
            {
                data.Add(entity);
            });
            mockSet.Setup(m => m.Remove(It.IsAny<DAL.Models.Task>())).Callback<DAL.Models.Task>(entity => data.Remove(entity));
            mockSet.Setup(m => m.Update(It.IsAny<DAL.Models.Task>())).Callback<DAL.Models.Task>(entity =>
            {
                var item = data.FirstOrDefault(i => i.TaskId == entity.TaskId);
                if (item != null)
                {
                    data.Remove(item);
                }
                data.Add(entity);
            });
            return mockSet;
        }

        [Test]
        public void GetEmployee_ExistingEmployee_ReturnsEmployee()
        {
            // Arrange
            int existingEmployeeId = 1;

            // Act
            var employee = _repository.GetEmployee(existingEmployeeId);

            // Assert
            Assert.IsNotNull(employee);
            Assert.AreEqual(existingEmployeeId, employee.EmployeeId);
        }

        [Test]
        public void GetEmployee_NonExistingEmployee_ReturnsNull()
        {
            // Arrange
            int nonExistingEmployeeId = 100;

            // Act
            var employee = _repository.GetEmployee(nonExistingEmployeeId);

            // Assert
            Assert.IsNull(employee);
        }

        [Test]
        public void GetEmployees_ReturnsEmployees()
        {
            // Act
            var employees = _repository.GetEmployees(10, 0);

            // Assert
            Assert.IsNotNull(employees);
            Assert.AreEqual(_employees.Count, employees.Count);
        }

        [Test]
        public void UpdateEmployee_ExistingEmployee_UpdatesEmployee()
        {
            // Arrange
            var employee = _repository.GetEmployee(1);
            employee.FirstName = "UpdatedFirstName";

            // Act
            var result = _repository.UpdateEmployee(employee);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("UpdatedFirstName", _employees.First(e => e.EmployeeId == 1).FirstName);
        }

        [Test]
        public void CreateEmployee_NewEmployee_CreatesEmployee()
        {
            // Arrange
            var newEmployee = new Muzej.DomainObjects.Employee
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com"
            };

            // Act
            var employeeId = _repository.CreateEmployee(newEmployee);

            // Assert
            Assert.AreEqual(-1, employeeId);
            Assert.IsTrue(_employees.Any(e => e.EmployeeId != employeeId));
        }

        [Test]
        public void DeleteEmployee_ExistingEmployee_DeletesEmployee()
        {
            // Arrange
            int existingEmployeeId = 1;

            // Act
            var result = _repository.DeleteEmployee(existingEmployeeId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(_employees.Any(e => e.EmployeeId == existingEmployeeId));
            Assert.IsFalse(_tasks.Any(t => t.EmployeeId == existingEmployeeId));
        }
    }
}
