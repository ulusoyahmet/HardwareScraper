using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareScrapper.Domain.Entities
{
    public class CPU : HardwareComponent
    {
        public int CoreCount { get; set; }

        public int ThreadCount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal BaseClock { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal BoostClock { get; set; }

        [MaxLength(50)]
        public string? Socket { get; set; }

        public int TDP { get; set; }

        [MaxLength(50)]
        public string? Architecture { get; set; }

        public int? CacheSize { get; set; }
    }
}
