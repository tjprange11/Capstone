using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoxAndHounds.Models
{
    public class WorkerScheduleDayJunction
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Worker")]
        public int WorkerId { get; set; }
        public Shift Worker { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("ScheduleDay")]
        public int ScheduleDayId { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
    }
}