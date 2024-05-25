using Muzej.DomainObjects;

namespace Muzej.Repository.Interfaces
{
    public interface IShiftTypesRepository
    {
        public ShiftType GetShiftType(int id);
        public ICollection<ShiftType> GetShiftTypes();
        public bool UpdateShiftType(ShiftType shiftType);
        public int CreateShiftType(ShiftType shiftType);
        public bool DeleteShiftType(int id);
    }
}
