using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HardwareScrapper.Domain.Abstractions;

namespace HardwareScrapper.Domain.Entities
{
    public class Specification : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Value { get; set; }

        // Foreign keys
        public int HardwareComponentId { get; set; }

        // Navigation properties
        [ForeignKey("HardwareComponentId")]
        public virtual HardwareComponent? HardwareComponent { get; set; }
    }
}
