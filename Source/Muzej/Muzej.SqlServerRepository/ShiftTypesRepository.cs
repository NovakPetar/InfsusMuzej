using Muzej.DAL.Models;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Muzej.SqlServerRepository
{
    public class ShiftTypesRepository : IShiftTypesRepository
    {
        private MUZContext _context;

        public ShiftTypesRepository(MUZContext context)
        {
            _context = context;
        }

        public DomainObjects.ShiftType GetShiftType(int id)
        {
            var shiftType = _context.ShiftTypes.FirstOrDefault(x => x.ShiftTypeId == id);
            if (shiftType == null) return null;
            return shiftType.Adapt<DomainObjects.ShiftType>();
        }

        public ICollection<DomainObjects.ShiftType> GetShiftTypes()
        {
            return _context.ShiftTypes
                .Adapt<ICollection<DomainObjects.ShiftType>>()
                .ToList();
        }

        public bool UpdateShiftType(DomainObjects.ShiftType shiftType)
        {
            try
            {
                _context.ShiftTypes.Update(shiftType.Adapt<Muzej.DAL.Models.ShiftType>());
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public int CreateShiftType(DomainObjects.ShiftType shiftType)
        {
            try
            {
                var newShiftType = _context.ShiftTypes.Add(shiftType.Adapt<Muzej.DAL.Models.ShiftType>());
                _context.SaveChanges();
                return newShiftType.Entity.ShiftTypeId;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteShiftType(int id)
        {
            try
            {
                var shiftType = _context.ShiftTypes.FirstOrDefault(x => x.ShiftTypeId == id);
                if (shiftType == null) return false;

                _context.ShiftTypes.Remove(shiftType);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
