using HardwareScrapper.Domain.Entities;
using HardwareScrapper.Services.Configs;
using HtmlAgilityPack;

namespace HardwareScrapper.Services.Abstractions
{
    public interface IScrapingStrategy
    {
        List<HardwareComponent> Extract(HtmlDocument document, string url, ScrapingConfig config,
            Dictionary<string, int> categoryMap, Dictionary<string, int> manufacturerMap);
    }
}
