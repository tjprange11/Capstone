using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoxAndHounds.Models
{
    public class AvailabilityDay
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Able to Work?")]
        public bool AbleToWork { get; set; }
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }
        
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }
    }
}