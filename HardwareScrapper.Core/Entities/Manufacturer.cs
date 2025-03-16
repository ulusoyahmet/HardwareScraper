using System.ComponentModel.DataAnnotations;
using HardwareScrapper.Domain.Abstractions;

namespace HardwareScrapper.Domain.Entities
{
    public class Manufacturer : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string LogoUrl { get; set; }

        [MaxLength(255)]
        public string WebsiteUrl { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<HardwareComponent> HardwareComponents { get; set; }
    }
}
