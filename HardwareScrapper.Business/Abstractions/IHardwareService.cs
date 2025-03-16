using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.Business.Abstractions
{
    public interface IHardwareService
    {
        // Basic CRUD operations
        HardwareComponent GetHardwareComponentById(int id);
        IEnumerable<HardwareComponent> GetAllHardwareComponents(int page = 1, int pageSize = 20);
        HardwareComponent AddHardwareComponent(HardwareComponent component);
        void UpdateHardwareComponent(HardwareComponent component);
        void DeleteHardwareComponent(int id);

        // Specialized queries
        IEnumerable<HardwareComponent> GetHardwareComponentsByCategory(int categoryId, int page = 1, int pageSize = 20);
        IEnumerable<HardwareComponent> GetHardwareComponentsByManufacturer(int manufacturerId, int page = 1, int pageSize = 20);
        IEnumerable<HardwareComponent> SearchHardwareComponents(string searchTerm, int page = 1, int pageSize = 20);

        // Type-specific operations
        IEnumerable<CPU> GetAllCPUs(int page = 1, int pageSize = 20);
        IEnumerable<GPU> GetAllGPUs(int page = 1, int pageSize = 20);
        IEnumerable<Motherboard> GetAllMotherboards(int page = 1, int pageSize = 20);
        IEnumerable<RAM> GetAllRAMs(int page = 1, int pageSize = 20);
        IEnumerable<Storage> GetAllStorages(int page = 1, int pageSize = 20);

        T GetComponentWithDetails<T>(int id) where T : HardwareComponent;

        // Statistics
        Dictionary<string, int> GetComponentCountsByCategory();
        Dictionary<string, int> GetComponentCountsByManufacturer();
    }
}
