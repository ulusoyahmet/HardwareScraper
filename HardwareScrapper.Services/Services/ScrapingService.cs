using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareScrapper.DataAccess.Abstractions;
using HardwareScrapper.Domain.Entities;
using HardwareScrapper.Services.Abstractions;
using HardwareScrapper.Services.Configs;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace HardwareScrapper.Services.Services
{
    public class ScrapingService : IScrapingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, IScrapingStrategy> _strategies;

        public ScrapingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            // Register scraping strategies
            _strategies = new Dictionary<string, IScrapingStrategy>
            {
                { "default", new BaseScrapingStrategy() }
                // You can add specialized strategies for specific websites
                // { "newegg", new NeweggScrapingStrategy() }
            };
        }

        public ScrapingLog ScrapeWebsite(int scrapingSourceId)
        {
            var source = _unitOfWork.ScrapingSourceRepository.GetById(scrapingSourceId);
            if (source == null || !source.IsEnabled)
                throw new ArgumentException($"Invalid or disabled scraping source: {scrapingSourceId}");

            // Create log entry
            var log = new ScrapingLog
            {
                ScrapingSourceId = scrapingSourceId,
                StartTime = DateTime.UtcNow,
                IsSuccessful = false,
                ItemsScraped = 0
            };

            try
            {
                // Get category and manufacturer lookups
                var categories = _unitOfWork.CategoryRepository.GetAll().ToDictionary(c => c.Name, c => c.Id);
                var manufacturers = _unitOfWork.ManufacturerRepository.GetAll().ToDictionary(m => m.Name, m => m.Id);

                // Ensure we have an "Unknown" manufacturer
                if (!manufacturers.ContainsKey("Unknown"))
                {
                    var unknown = new Manufacturer { Name = "Unknown", Description = "Unknown Manufacturer" };
                    _unitOfWork.ManufacturerRepository.Add(unknown);
                    manufacturers.Add("Unknown", unknown.Id);
                }

                // Parse configuration
                var config = JsonConvert.DeserializeObject<ScrapingConfig>(source.ScrapeConfiguration);

                // Download and parse the HTML
                string html = _httpClient.GetStringAsync(source.BaseUrl).Result;
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Select the appropriate strategy
                string strategyKey = source.Name.ToLower().Replace(" ", "");
                var strategy = _strategies.ContainsKey(strategyKey) ? _strategies[strategyKey] : _strategies["default"];

                // Extract components
                var components = strategy.Extract(doc, source.BaseUrl, config, categories, manufacturers);

                // Save components to database
                if (components.Any())
                {
                    _unitOfWork.HardwareRepository.AddRange(components);
                    log.ItemsScraped = components.Count;
                    log.IsSuccessful = true;
                }

                return log;
            }
            catch (Exception ex)
            {
                log.ErrorMessage = ex.Message;
                return log;
            }
            finally
            {
                // Complete the log
                log.EndTime = DateTime.UtcNow;
                _unitOfWork.ScrapingLogRepository.Add(log);
            }
        }

        public async Task<ScrapingLog> ScrapeWebsiteAsync(int scrapingSourceId)
        {
            var source = _unitOfWork.ScrapingSourceRepository.GetById(scrapingSourceId);
            if (source == null || !source.IsEnabled)
                throw new ArgumentException($"Invalid or disabled scraping source: {scrapingSourceId}");

            // Create log entry
            var log = new ScrapingLog
            {
                ScrapingSourceId = scrapingSourceId,
                StartTime = DateTime.UtcNow,
                IsSuccessful = false,
                ItemsScraped = 0
            };

            try
            {
                // Get category and manufacturer lookups
                var categories = _unitOfWork.CategoryRepository.GetAll().ToDictionary(c => c.Name, c => c.Id);
                var manufacturers = _unitOfWork.ManufacturerRepository.GetAll().ToDictionary(m => m.Name, m => m.Id);

                // Ensure we have an "Unknown" manufacturer
                if (!manufacturers.ContainsKey("Unknown"))
                {
                    var unknown = new Manufacturer { Name = "Unknown", Description = "Unknown Manufacturer" };
                    _unitOfWork.ManufacturerRepository.Add(unknown);
                    manufacturers.Add("Unknown", unknown.Id);
                }

                // Parse configuration
                var config = JsonConvert.DeserializeObject<ScrapingConfig>(source.ScrapeConfiguration);

                // Download and parse the HTML
                string html = await _httpClient.GetStringAsync(source.BaseUrl);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Select the appropriate strategy
                string strategyKey = source.Name.ToLower().Replace(" ", "");
                var strategy = _strategies.ContainsKey(strategyKey) ? _strategies[strategyKey] : _strategies["default"];

                // Extract components
                var components = strategy.Extract(doc, source.BaseUrl, config, categories, manufacturers);

                // Save components to database
                if (components.Any())
                {
                    _unitOfWork.HardwareRepository.AddRange(components);
                    log.ItemsScraped = components.Count;
                    log.IsSuccessful = true;
                }

                return log;
            }
            catch (Exception ex)
            {
                log.ErrorMessage = ex.Message;
                return log;
            }
            finally
            {
                // Complete the log
                log.EndTime = DateTime.UtcNow;
                _unitOfWork.ScrapingLogRepository.Add(log);
            }
        }

        public List<ScrapingSource> GetAllScrapingSources()
        {
            return _unitOfWork.ScrapingSourceRepository.GetAll().ToList();
        }

        public ScrapingSource GetScrapingSourceById(int id)
        {
            return _unitOfWork.ScrapingSourceRepository.GetById(id);
        }
    }
}

