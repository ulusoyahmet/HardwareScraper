using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareScrapper.Domain.Abstractions;

namespace HardwareScrapper.Domain.Entities
{
    public class ScrapingLog : BaseEntity
    {
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsSuccessful { get; set; }

        public int ItemsScraped { get; set; }

        [MaxLength(5000)]
        public string? ErrorMessage { get; set; }

        // Foreign keys
        public int ScrapingSourceId { get; set; }

        // Navigation properties
        [ForeignKey("ScrapingSourceId")]
        public virtual ScrapingSource? ScrapingSource { get; set; }
    }
}
