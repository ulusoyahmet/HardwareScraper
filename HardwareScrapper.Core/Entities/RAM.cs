using System.ComponentModel.DataAnnotations;

namespace HardwareScrapper.Domain.Entities
{
    public class RAM : HardwareComponent
    {
        public int Capacity { get; set; }

        [MaxLength(50)]
        public string? MemoryType { get; set; }

        public int Speed { get; set; }

        public int CASLatency { get; set; }

        public int ModuleCount { get; set; }

        [MaxLength(50)]
        public string? Timings { get; set; }

        public bool HasRGB { get; set; }
    }

}
