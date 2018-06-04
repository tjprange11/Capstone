using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoxAndHounds.Models
{
    public class BusserScheduleDayJunction
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Busser")]
        public int BusserId { get; set; }
        public Shift Busser { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("ScheduleDay")]
        public int ScheduleDayId { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
    }
}