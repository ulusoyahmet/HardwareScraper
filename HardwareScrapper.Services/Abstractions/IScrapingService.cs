using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.Services.Abstractions
{
    public interface IScrapingService
    {
        ScrapingLog ScrapeWebsite(int scrapingSourceId);
        Task<ScrapingLog> ScrapeWebsiteAsync(int scrapingSourceId);
        List<ScrapingSource> GetAllScrapingSources();
        ScrapingSource GetScrapingSourceById(int id);
    }
}
