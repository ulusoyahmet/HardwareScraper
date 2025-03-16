using HardwareScrapper.Business.Abstractions;
using HardwareScrapper.DataAccess.Abstractions;
using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.Business.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManufacturerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Manufacturer GetManufacturerById(int id)
        {
            return _unitOfWork.ManufacturerRepository.GetById(id);
        }

        public IEnumerable<Manufacturer> GetAllManufacturers()
        {
            return _unitOfWork.ManufacturerRepository.GetAll();
        }

        public Manufacturer AddManufacturer(Manufacturer manufacturer)
        {
            return _unitOfWork.ManufacturerRepository.Add(manufacturer);
        }

        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            _unitOfWork.ManufacturerRepository.Update(manufacturer);
        }

        public void DeleteManufacturer(int id)
        {
            _unitOfWork.ManufacturerRepository.Delete(id);
        }
    }
}
