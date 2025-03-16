using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.DataAccess.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Manufacturer> ManufacturerRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IHardwareRepository HardwareRepository { get; }
        IRepository<Specification> SpecificationRepository { get; }
        IRepository<PriceHistory> PriceHistoryRepository { get; }
        IRepository<Review> ReviewRepository { get; }
        IRepository<ScrapingSource> ScrapingSourceRepository { get; }
        IRepository<ScrapingLog> ScrapingLogRepository { get; }
        int Complete();
    }
}
