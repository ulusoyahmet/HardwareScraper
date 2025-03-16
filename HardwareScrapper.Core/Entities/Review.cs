using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HardwareScrapper.Domain.Abstractions;

namespace HardwareScrapper.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        [MaxLength(100)]
        public string? AuthorName { get; set; }

        public DateTime ReviewDate { get; set; }

        // Foreign keys
        public int HardwareComponentId { get; set; }

        // Navigation properties
        [ForeignKey("HardwareComponentId")]
        public virtual HardwareComponent? HardwareComponent { get; set; }
    }
}
