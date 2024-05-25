using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;

namespace Muzej.BLL
{
    public class ShiftTypeBLL
    {
        private IShiftTypesRepository _shiftTypeRepository;

        public ShiftTypeBLL(IRepositoryWrapper repositoryWrapper)
        {
            _shiftTypeRepository = repositoryWrapper.ShiftTypes;
        }

        public ShiftType GetShiftType(int id)
        {
            return _shiftTypeRepository.GetShiftType(id);
        }

        public ICollection<ShiftType> GetShiftTypes()
        {
            return _shiftTypeRepository.GetShiftTypes();
        }

        public bool UpdateShiftType(ShiftType shiftType)
        {
            return _shiftTypeRepository.UpdateShiftType(shiftType);
        }

        public int CreateShiftType(ShiftType shiftType)
        {
            return _shiftTypeRepository.CreateShiftType(shiftType);
        }

        public bool DeleteShiftType(int id)
        {
            return _shiftTypeRepository.DeleteShiftType(id);
        }
    }
}
