using System.ComponentModel.DataAnnotations;

namespace HardwareScrapper.Domain.Entities
{
    public class Motherboard : HardwareComponent
    {
        [MaxLength(50)]
        public string? FormFactor { get; set; }

        [MaxLength(50)]
        public string? Socket { get; set; }

        [MaxLength(50)]
        public string? Chipset { get; set; }

        public int MemorySlots { get; set; }

        [MaxLength(50)]
        public string? MemoryType { get; set; }

        public int MaxMemory { get; set; }

        [MaxLength(200)]
        public string? StorageInterfaces { get; set; }

        [MaxLength(100)]
        public string? ExpansionSlots { get; set; }
    }
}
