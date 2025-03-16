using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.Business.Abstractions
{
    public interface IManufacturerService
    {
        Manufacturer GetManufacturerById(int id);
        IEnumerable<Manufacturer> GetAllManufacturers();
        Manufacturer AddManufacturer(Manufacturer manufacturer);
        void UpdateManufacturer(Manufacturer manufacturer);
        void DeleteManufacturer(int id);
    }
}
