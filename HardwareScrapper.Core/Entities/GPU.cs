using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareScrapper.Domain.Entities
{
    public class GPU : HardwareComponent
    {
        public int MemorySize { get; set; }

        [MaxLength(50)]
        public string MemoryType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CoreClock { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal BoostClock { get; set; }

        public int? StreamProcessors { get; set; }

        [MaxLength(50)]
        public string? Interface { get; set; }

        public int TDP { get; set; }

        [MaxLength(100)]
        public string? Architecture { get; set; }

        [MaxLength(100)]
        public string? Ports { get; set; }
    }
}
