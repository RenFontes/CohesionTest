using CohesionTest.Db.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CohesionTest.Models
{
    public record ServiceRequest
    {
        public Guid? Id { get; init; }

        [Required]
        public string BuildingCode { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public CurrentStatusEnum CurrentStatus { get; init; }

        [Required]
        public string CreatedBy { get; init; }

        [Required]
        public DateTime CreatedDate { get; init; }

        [Required]
        public string LastModifiedBy { get; init; }

        [Required]
        public DateTime LastModifiedDate { get; init; }
    }
}
