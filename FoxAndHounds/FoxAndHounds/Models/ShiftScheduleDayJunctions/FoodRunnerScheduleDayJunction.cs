using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoxAndHounds.Models
{
    public class FoodRunnerScheduleDayJunction
    {
        [Key, Column(Order = 0)]
        [ForeignKey("FoodRunner")]
        public int FoodRunnerId { get; set; }
        public Shift FoodRunner { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("ScheduleDay")]
        public int ScheduleDayId { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
    }
}