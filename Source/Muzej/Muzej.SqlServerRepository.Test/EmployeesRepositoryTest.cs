using NUnit.Framework;
using Moq;
using Muzej.SqlServerRepository;
using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Muzej.SqlServerRepository.Tests
{
    [TestFixture]
    public class EmployeesRepositoryTests
    {
        private IEmployeesRepository _repository;
        private Mock<MUZContext> _mockContext;
        private List<Employee> _employees;
        
        [SetUp]
        public void Setup()
        {
            _employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
                new Employee { EmployeeId = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com" }
            };

            _mockContext = new Mock<MUZContext>();

            // Setup DbSet<Employee> to return a mock DbSet<Employee> that represents the data in _employees
            var mockSet = new Mock<DbSet<Employee>>();
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(_employees.AsQueryable().Provider);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(_employees.AsQueryable().Expression);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(_employees.AsQueryable().ElementType);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(_employees.AsQueryable().GetEnumerator());

            // Setup Add and Remove methods of DbSet<Employee> to update the _employees list
            mockSet.Setup(m => m.Add(It.IsAny<Employee>())).Callback<Employee>((entity) => _employees.Add(entity));
            mockSet.Setup(m => m.Remove(It.IsAny<Employee>())).Callback<Employee>((entity) => _employees.Remove(entity));

            _mockContext.Setup(m => m.Employees).Returns(mockSet.Object);

            _repository = new EmployeesRepository(_mockContext.Object);
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
            var newEmployee = new DomainObjects.Employee
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com"
            };

            // Act
            var employeeId = _repository.CreateEmployee(newEmployee);

            // Assert
            Assert.AreNotEqual(-1, employeeId);
            Assert.IsTrue(_employees.Any(e => e.EmployeeId == employeeId));
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
        }
    }
}
