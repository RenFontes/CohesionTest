using CohesionTest.Db.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CohesionTest.Db
{
    public class ServiceRequest
    {
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("buildingCode")]
        public string BuildingCode { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("currentStatus")]
        public CurrentStatusEnum CurrentStatus { get; set; }

        [Required]
        [Column("createdBy")]
        public string CreatedBy { get; set; }

        [Required]
        [Column("createdDate")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column("lastModifiedBy")]
        public string LastModifiedBy { get; set; }

        [Required]
        [Column("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }
    }
}
