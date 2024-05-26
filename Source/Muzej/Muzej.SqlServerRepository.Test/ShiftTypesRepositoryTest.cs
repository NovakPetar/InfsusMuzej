using NUnit.Framework;
using Moq;
using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using Muzej.SqlServerRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Muzej.SqlServerRepository.Tests
{
    [TestFixture]
    public class ShiftTypesRepositoryTests
    {
        private IShiftTypesRepository _repository;
        private Mock<MUZContext> _mockContext;
        private List<ShiftType> _shiftTypes;

        [SetUp]
        public void Setup()
        {
            _shiftTypes = new List<ShiftType>
            {
                new ShiftType { ShiftTypeId = 1, ShiftTypeName = "Morning" },
                new ShiftType { ShiftTypeId = 2, ShiftTypeName = "Evening" }
            };

            _mockContext = new Mock<MUZContext>();

            var mockSet = CreateMockShiftTypeDbSet(_shiftTypes);

            _mockContext.Setup(m => m.ShiftTypes).Returns(mockSet.Object);

            _repository = new ShiftTypesRepository(_mockContext.Object);
        }

        [Test]
        public void GetShiftType_ExistingShiftType_ReturnsShiftType()
        {
            int existingShiftTypeId = 1;

            var shiftType = _repository.GetShiftType(existingShiftTypeId);

            Assert.IsNotNull(shiftType);
            Assert.AreEqual(existingShiftTypeId, shiftType.ShiftTypeId);
        }

        [Test]
        public void GetShiftType_NonExistingShiftType_ReturnsNull()
        {
            int nonExistingShiftTypeId = 100;

            var shiftType = _repository.GetShiftType(nonExistingShiftTypeId);

            Assert.IsNull(shiftType);
        }

        [Test]
        public void GetShiftTypes_ReturnsShiftTypes()
        {
            var shiftTypes = _repository.GetShiftTypes();

            Assert.IsNotNull(shiftTypes);
            Assert.AreEqual(_shiftTypes.Count, shiftTypes.Count);
        }

        [Test]
        public void UpdateShiftType_ExistingShiftType_UpdatesShiftType()
        {
            var shiftType = _repository.GetShiftType(1);
            shiftType.ShiftTypeName = "UpdatedShiftTypeName";

            var result = _repository.UpdateShiftType(shiftType);

            Assert.IsTrue(result);
            Assert.AreEqual("UpdatedShiftTypeName", _shiftTypes.First(e => e.ShiftTypeId == 1).ShiftTypeName);
        }

        [Test]
        public void CreateShiftType_NewShiftType_CreatesShiftType()
        {
            var newShiftType = new DomainObjects.ShiftType
            {
                ShiftTypeId = 3,
                ShiftTypeName = "New Shift"
            };

            _repository.CreateShiftType(newShiftType);

            Assert.IsTrue(_shiftTypes.Any(e => e.ShiftTypeId == newShiftType.ShiftTypeId));
        }

        [Test]
        public void DeleteShiftType_ExistingShiftType_DeletesShiftType()
        {
            int existingShiftTypeId = 1;

            var result = _repository.DeleteShiftType(existingShiftTypeId);

            Assert.IsTrue(result);
            Assert.IsFalse(_shiftTypes.Any(e => e.ShiftTypeId == existingShiftTypeId));
        }

        public static Mock<DbSet<ShiftType>> CreateMockShiftTypeDbSet(List<ShiftType> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<ShiftType>>();
            mockSet.As<IQueryable<ShiftType>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<ShiftType>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<ShiftType>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<ShiftType>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<ShiftType>())).Callback<ShiftType>(entity =>
            {
                data.Add(entity);
            });
            mockSet.Setup(m => m.Remove(It.IsAny<ShiftType>())).Callback<ShiftType>(entity => data.Remove(entity));
            mockSet.Setup(m => m.Update(It.IsAny<ShiftType>())).Callback<ShiftType>(entity =>
            {
                var item = data.FirstOrDefault(i => i.ShiftTypeId == entity.ShiftTypeId);
                if (item != null)
                {
                    data.Remove(item);
                }
                data.Add(entity);
            });
            return mockSet;
        }
    }
}
