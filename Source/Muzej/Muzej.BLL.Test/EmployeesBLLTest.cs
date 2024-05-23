// File: Muzej.Tests/EmployeesBLLTests.cs
using NUnit.Framework;
using Moq;
using Muzej.BLL;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Muzej.BLL.Tests
{
    [TestFixture]
    public class EmployeesBLLTest
    {
        private EmployeesBLL _employeesBLL;
        private Mock<IEmployeesRepository> _mockEmployeesRepository;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        [SetUp]
        public void Setup()
        {
            _mockEmployeesRepository = new Mock<IEmployeesRepository>();
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            
            _mockRepositoryWrapper.Setup(r => r.Employees).Returns(_mockEmployeesRepository.Object);
            
            _employeesBLL = new EmployeesBLL(_mockRepositoryWrapper.Object);
        }

        [Test]
        public void GetEmployee_ShouldReturnEmployee_WhenEmployeeExists()
        {
            var employeeId = 1;
            var expectedEmployee = new Employee { EmployeeId = employeeId };
            _mockEmployeesRepository.Setup(r => r.GetEmployee(employeeId)).Returns(expectedEmployee);
            
            var result = _employeesBLL.GetEmployee(employeeId);
            
            Assert.AreEqual(expectedEmployee, result);
        }

        [Test]
        public void GetEmployees_ShouldReturnEmployees_WhenEmployeesExist()
        {
            var count = 5;
            var offset = 0;
            var expectedEmployees = new List<Employee>
            {
                new Employee { EmployeeId = 1 },
                new Employee { EmployeeId = 2 }
            };
            _mockEmployeesRepository.Setup(r => r.GetEmployees(count, offset)).Returns(expectedEmployees);  
            
            var result = _employeesBLL.GetEmployees(count, offset);

            Assert.AreEqual(expectedEmployees, result);
        }

        [Test]
        public void UpdateEmployee_ShouldReturnTrue_WhenEmployeeIsUpdated()
        {
            var employee = new Employee { EmployeeId = 1 };
            _mockEmployeesRepository.Setup(r => r.UpdateEmployee(employee)).Returns(true);
            
            var result = _employeesBLL.UpdateEmployee(employee);
            
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateEmployee_ShouldReturnEmployeeId_WhenEmployeeIsCreated()
        {
            var employee = new Employee { EmployeeId = 1 };
            var expectedEmployeeId = 1;
            _mockEmployeesRepository.Setup(r => r.CreateEmployee(employee)).Returns(expectedEmployeeId);
            
            var result = _employeesBLL.CreateEmployee(employee);
            
            Assert.AreEqual(expectedEmployeeId, result);
        }

        [Test]
        public void DeleteEmployee_ShouldReturnTrue_WhenEmployeeIsDeleted()
        {
            var employeeId = 1;
            _mockEmployeesRepository.Setup(r => r.DeleteEmployee(employeeId)).Returns(true);
            
            var result = _employeesBLL.DeleteEmployee(employeeId);
            
            Assert.IsTrue(result);
        }
    }
}
