using System.ComponentModel.DataAnnotations;

namespace HardwareScrapper.Domain.Entities
{
    public class Storage : HardwareComponent
    {
        public int Capacity { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; } // SSD, HDD, NVMe

        [MaxLength(50)]
        public string? FormFactor { get; set; } // 2.5", 3.5", M.2, etc.

        [MaxLength(50)]
        public string? Interface { get; set; } // SATA, PCIe, etc.

        public int? ReadSpeed { get; set; }

        public int? WriteSpeed { get; set; }

        public int? TBW { get; set; } // Terabytes Written (endurance)

        [MaxLength(100)]
        public string? CacheSize { get; set; }
    }
}
