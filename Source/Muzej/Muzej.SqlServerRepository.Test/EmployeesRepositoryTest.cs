using NUnit.Framework;
using Muzej.SqlServerRepository;
using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Mapster;
using System.Linq;

namespace Muzej.SqlServerRepository.Tests
{
    [TestFixture]
    public class EmployeesRepositoryTests
    {
        private IEmployeesRepository _repository;
        private DbContextOptions<MUZContext> _options;
        private SqliteConnection _connection;

        [SetUp]
        public void Setup()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            // Tu nesto ne radi, pogledaj kad stignes
            _options = new DbContextOptionsBuilder<MUZContext>()
               .UseSqlServer(_connection)
                .Options;

            using (var context = new MUZContext(_options))
            {
                context.Database.EnsureCreated();
                // Seed some test data
                context.Employees.Add(new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" });
                context.SaveChanges();
            }

            _repository = new EmployeesRepository(new MUZContext(_options));
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
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
            Assert.AreEqual(1, employees.Count);
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
            using (var context = new MUZContext(_options))
            {
                var updatedEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                Assert.IsNotNull(updatedEmployee);
                Assert.AreEqual("UpdatedFirstName", updatedEmployee.FirstName);
            }
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
            using (var context = new MUZContext(_options))
            {
                var createdEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
                Assert.IsNotNull(createdEmployee);
                Assert.AreEqual("Jane", createdEmployee.FirstName);
                Assert.AreEqual("Doe", createdEmployee.LastName);
                Assert.AreEqual("jane@example.com", createdEmployee.Email);
            }
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
            using (var context = new MUZContext(_options))
            {
                var deletedEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == existingEmployeeId);
                Assert.IsNull(deletedEmployee);
            }
        }
    }
}