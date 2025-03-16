using HardwareScrapper.DataAccess.Abstractions;
using HardwareScrapper.DataAccess.Contexts;
using HardwareScrapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HardwareScrapper.DataAccess.Repositories
{
    public class HardwareRepository : Repository<HardwareComponent>, IHardwareRepository
    {
        public HardwareRepository(HardwareDbContext context) : base(context)
        {
        }

        public IEnumerable<HardwareComponent> GetByCategory(int categoryId, int page = 1, int pageSize = 20)
        {
            return _dbSet
                .Where(c => c.IsActive && c.CategoryId == categoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<HardwareComponent> GetByManufacturer(int manufacturerId, int page = 1, int pageSize = 20)
        {
            return _dbSet
                .Where(c => c.IsActive && c.ManufacturerId == manufacturerId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<HardwareComponent> Search(string searchTerm, int page = 1, int pageSize = 20)
        {
            searchTerm = searchTerm.ToLower();

            return _dbSet
                .Where(c => c.IsActive &&
                    (c.Name.ToLower().Contains(searchTerm) ||
                     c.Model.ToLower().Contains(searchTerm) ||
                     c.Manufacturer.Name.ToLower().Contains(searchTerm)))
                .Include(c => c.Manufacturer)
                .Include(c => c.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<T> GetSpecificType<T>(int page = 1, int pageSize = 20) where T : HardwareComponent
        {
            return _context.Set<T>()
                .Where(c => c.IsActive)
                .Skip((page - 1) * pageSize)
            .Take(pageSize)
                .ToList();
        }

        public T GetComponentByIdWithDetails<T>(int id) where T : HardwareComponent
        {
            return _context.Set<T>()
                .Where(c => c.Id == id && c.IsActive)
                .Include(c => c.Manufacturer)
                .Include(c => c.Category)
                .Include(c => c.Specifications)
                .Include(c => c.PriceHistory)
                .Include(c => c.Reviews)
                .FirstOrDefault();
        }
    }
}
