using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HardwareScrapper.Domain.Abstractions;

namespace HardwareScrapper.Domain.Entities
{
    public class PriceHistory : BaseEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? Currency { get; set; }

        public DateTime RecordedDate { get; set; }

        [MaxLength(200)]
        public string? RetailerName { get; set; }

        [MaxLength(1000)]
        public string? RetailerUrl { get; set; }

        // Foreign keys
        public int HardwareComponentId { get; set; }

        // Navigation properties
        [ForeignKey("HardwareComponentId")]
        public virtual HardwareComponent? HardwareComponent { get; set; }
    }
}
