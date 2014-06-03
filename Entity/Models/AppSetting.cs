using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BombVacuum.Entity.Models
{
    [Table("AppSettings")]
    public class AppSetting
    {
        [MaxLength(50), Index, Key, Column(TypeName = "varchar")]
        public string Property { get; set; }
        public string Value { get; set; }
    }
}