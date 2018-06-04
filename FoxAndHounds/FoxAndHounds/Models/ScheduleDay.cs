using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoxAndHounds.Models
{
    public class ScheduleDay
    {
        [Key]
        public int Id { get; set; }
        public DateTime Day { get; set; }
    }
}