using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;

namespace Muzej.BLL.Tests
{
    [TestFixture]
    public class ShiftTypeBLLTests
    {
        private ShiftTypeBLL _shiftTypeBLL;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IShiftTypesRepository> _mockShiftTypesRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockShiftTypesRepository = new Mock<IShiftTypesRepository>();
            _mockRepositoryWrapper.Setup(r => r.ShiftTypes).Returns(_mockShiftTypesRepository.Object);
            _shiftTypeBLL = new ShiftTypeBLL(_mockRepositoryWrapper.Object);
        }

        [Test]
        public void GetShiftType_ShouldReturnShiftType_WhenShiftTypeExists()
        {
            // Arrange
            int shiftTypeId = 1;
            var expectedShiftType = new ShiftType { ShiftTypeId = shiftTypeId, ShiftTypeName = "Morning" };
            _mockShiftTypesRepository.Setup(r => r.GetShiftType(shiftTypeId)).Returns(expectedShiftType);

            // Act
            var result = _shiftTypeBLL.GetShiftType(shiftTypeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedShiftType, result);
        }

        [Test]
        public void GetShiftTypes_ShouldReturnListOfShiftTypes()
        {
            // Arrange
            var expectedShiftTypes = new List<ShiftType>
            {
                new ShiftType { ShiftTypeId = 1, ShiftTypeName = "Morning" },
                new ShiftType { ShiftTypeId = 2, ShiftTypeName = "Evening" }
            };
            _mockShiftTypesRepository.Setup(r => r.GetShiftTypes()).Returns(expectedShiftTypes);

            // Act
            var result = _shiftTypeBLL.GetShiftTypes();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedShiftTypes.Count, result.Count);
        }

        [Test]
        public void UpdateShiftType_ShouldReturnTrue_WhenShiftTypeIsUpdated()
        {
            // Arrange
            var shiftType = new ShiftType { ShiftTypeId = 1, ShiftTypeName = "Morning" };
            _mockShiftTypesRepository.Setup(r => r.UpdateShiftType(shiftType)).Returns(true);

            // Act
            var result = _shiftTypeBLL.UpdateShiftType(shiftType);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateShiftType_ShouldReturnShiftTypeId_WhenShiftTypeIsCreated()
        {
            // Arrange
            var shiftType = new ShiftType { ShiftTypeName = "Morning" };
            int expectedShiftTypeId = 1;
            _mockShiftTypesRepository.Setup(r => r.CreateShiftType(shiftType)).Returns(expectedShiftTypeId);

            // Act
            var result = _shiftTypeBLL.CreateShiftType(shiftType);

            // Assert
            Assert.AreEqual(expectedShiftTypeId, result);
        }

        [Test]
        public void DeleteShiftType_ShouldReturnTrue_WhenShiftTypeIsDeleted()
        {
            // Arrange
            int shiftTypeId = 1;
            _mockShiftTypesRepository.Setup(r => r.DeleteShiftType(shiftTypeId)).Returns(true);

            // Act
            var result = _shiftTypeBLL.DeleteShiftType(shiftTypeId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
