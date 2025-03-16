using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.DataAccess.Abstractions
{
    public interface IHardwareRepository : IRepository<HardwareComponent>
    {
        IEnumerable<HardwareComponent> GetByCategory(int categoryId, int page = 1, int pageSize = 20);
        IEnumerable<HardwareComponent> GetByManufacturer(int manufacturerId, int page = 1, int pageSize = 20);
        IEnumerable<HardwareComponent> Search(string searchTerm, int page = 1, int pageSize = 20);
        IEnumerable<T> GetSpecificType<T>(int page = 1, int pageSize = 20) where T : HardwareComponent;
        T GetComponentByIdWithDetails<T>(int id) where T : HardwareComponent;
    }
}
