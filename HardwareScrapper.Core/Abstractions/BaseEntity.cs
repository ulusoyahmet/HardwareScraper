﻿using System.ComponentModel.DataAnnotations;

namespace HardwareScrapper.Domain.Abstractions
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
