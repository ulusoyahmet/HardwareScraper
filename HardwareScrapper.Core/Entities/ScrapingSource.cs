using System.ComponentModel.DataAnnotations;
using HardwareScrapper.Domain.Abstractions;

namespace HardwareScrapper.Domain.Entities
{
    public class ScrapingSource : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? BaseUrl { get; set; }

        [MaxLength(1000)]
        public string? LogoUrl { get; set; }

        public bool IsEnabled { get; set; } = true;

        [MaxLength(5000)]
        public string? ScrapeConfiguration { get; set; } // JSON configuration for scraping this source

        // Navigation properties
        public virtual ICollection<ScrapingLog>? ScrapingLogs { get; set; }
    }
}
