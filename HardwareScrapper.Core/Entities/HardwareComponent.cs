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
    public class HardwareComponent : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Model { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(100)]
        public string? Currency { get; set; } = "USD";

        [Required]
        [MaxLength(1000)]
        public string? SourceUrl { get; set; }

        [MaxLength(1000)]
        public string? ImageUrl { get; set; }

        public DateTime? ScrapedDate { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int? Rating { get; set; }

        [MaxLength(100)]
        public string? SKU { get; set; }

        public bool InStock { get; set; }

        // Foreign keys
        public int ManufacturerId { get; set; }

        public int CategoryId { get; set; }

        // Navigation properties
        [ForeignKey("ManufacturerId")]
        public virtual Manufacturer? Manufacturer { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<Specification>? Specifications { get; set; }

        public virtual ICollection<PriceHistory>? PriceHistory { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
