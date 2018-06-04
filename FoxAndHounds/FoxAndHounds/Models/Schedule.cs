using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoxAndHounds.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Monday")]
        public int? MondayId { get; set; }
        public ScheduleDay Monday { get; set; }

        [ForeignKey("Tuesday")]
        public int? TuesdayId { get; set; }
        public ScheduleDay Tuesday { get; set; }

        [ForeignKey("Wednesday")]
        public int? WednesdayId { get; set; }
        public ScheduleDay Wednesday { get; set; }

        [ForeignKey("Thursday")]
        public int? ThursdayId { get; set; }
        public ScheduleDay Thursday { get; set; }

        [ForeignKey("Friday")]
        public int ?FridayId { get; set; }
        public ScheduleDay Friday { get; set; }

        [ForeignKey("Saturday")]
        public int? SaturdayId { get; set; }
        public ScheduleDay Saturday { get; set; }

        [ForeignKey("Sunday")]
        public int? SundayId { get; set; }
        public ScheduleDay Sunday { get; set; }
    }
}