using HardwareScrapper.Business.Abstractions;
using HardwareScrapper.DataAccess.Abstractions;
using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.Business.Services
{
    public class HardwareService : IHardwareService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HardwareService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public HardwareComponent GetHardwareComponentById(int id)
        {
            return _unitOfWork.HardwareRepository.GetById(id);
        }

        public IEnumerable<HardwareComponent> GetAllHardwareComponents(int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.GetPaged(page, pageSize);
        }

        public HardwareComponent AddHardwareComponent(HardwareComponent component)
        {
            return _unitOfWork.HardwareRepository.Add(component);
        }

        public void UpdateHardwareComponent(HardwareComponent component)
        {
            _unitOfWork.HardwareRepository.Update(component);
        }

        public void DeleteHardwareComponent(int id)
        {
            _unitOfWork.HardwareRepository.Delete(id);
        }

        public IEnumerable<HardwareComponent> GetHardwareComponentsByCategory(int categoryId, int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.GetByCategory(categoryId, page, pageSize);
        }

        public IEnumerable<HardwareComponent> GetHardwareComponentsByManufacturer(int manufacturerId, int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.GetByManufacturer(manufacturerId, page, pageSize);
        }

        public IEnumerable<HardwareComponent> SearchHardwareComponents(string searchTerm, int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.Search(searchTerm, page, pageSize);
        }

        public IEnumerable<CPU> GetAllCPUs(int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.GetSpecificType<CPU>(page, pageSize);
        }

        public IEnumerable<GPU> GetAllGPUs(int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.GetSpecificType<GPU>(page, pageSize);
        }

        public IEnumerable<Motherboard> GetAllMotherboards(int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.GetSpecificType<Motherboard>(page, pageSize);
        }

        public IEnumerable<RAM> GetAllRAMs(int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.GetSpecificType<RAM>(page, pageSize);
        }

        public IEnumerable<Storage> GetAllStorages(int page = 1, int pageSize = 20)
        {
            return _unitOfWork.HardwareRepository.GetSpecificType<Storage>(page, pageSize);
        }

        public T GetComponentWithDetails<T>(int id) where T : HardwareComponent
        {
            return _unitOfWork.HardwareRepository.GetComponentByIdWithDetails<T>(id);
        }

        public Dictionary<string, int> GetComponentCountsByCategory()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();
            var result = new Dictionary<string, int>();

            foreach (var category in categories)
            {
                var count = _unitOfWork.HardwareRepository.Count(h => h.CategoryId == category.Id);
                result.Add(category.Name, count);
            }

            return result;
        }

        public Dictionary<string, int> GetComponentCountsByManufacturer()
        {
            var manufacturers = _unitOfWork.ManufacturerRepository.GetAll();
            var result = new Dictionary<string, int>();

            foreach (var manufacturer in manufacturers)
            {
                var count = _unitOfWork.HardwareRepository.Count(h => h.ManufacturerId == manufacturer.Id);
                result.Add(manufacturer.Name, count);
            }

            return result;
        }
    }
}
