using HardwareScrapper.DataAccess.Abstractions;
using HardwareScrapper.DataAccess.Contexts;
using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HardwareDbContext _context;
        private IRepository<Manufacturer> _manufacturerRepository;
        private IRepository<Category> _categoryRepository;
        private IHardwareRepository _hardwareRepository;
        private IRepository<Specification> _specificationRepository;
        private IRepository<PriceHistory> _priceHistoryRepository;
        private IRepository<Review> _reviewRepository;
        private IRepository<ScrapingSource> _scrapingSourceRepository;
        private IRepository<ScrapingLog> _scrapingLogRepository;

        public UnitOfWork(HardwareDbContext context)
        {
            _context = context;
        }

        public IRepository<Manufacturer> ManufacturerRepository =>
            _manufacturerRepository ??= new Repository<Manufacturer>(_context);

        public IRepository<Category> CategoryRepository =>
            _categoryRepository ??= new Repository<Category>(_context);

        public IHardwareRepository HardwareRepository =>
            _hardwareRepository ??= new HardwareRepository(_context);

        public IRepository<Specification> SpecificationRepository =>
            _specificationRepository ??= new Repository<Specification>(_context);

        public IRepository<PriceHistory> PriceHistoryRepository =>
            _priceHistoryRepository ??= new Repository<PriceHistory>(_context);

        public IRepository<Review> ReviewRepository =>
            _reviewRepository ??= new Repository<Review>(_context);

        public IRepository<ScrapingSource> ScrapingSourceRepository =>
            _scrapingSourceRepository ??= new Repository<ScrapingSource>(_context);

        public IRepository<ScrapingLog> ScrapingLogRepository =>
            _scrapingLogRepository ??= new Repository<ScrapingLog>(_context);

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
