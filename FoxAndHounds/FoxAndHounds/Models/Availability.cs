using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoxAndHounds.Models
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey("Monday")]
        public int? MondayId { get; set; }
        public AvailabilityDay Monday { get; set; }

        [ForeignKey("Tuesday")]
        public int? TuesdayId { get; set; }
        public AvailabilityDay Tuesday { get; set; }

        [ForeignKey("Wednesday")]
        public int? WednesdayId { get; set; }
        public AvailabilityDay Wednesday { get; set; }

        [ForeignKey("Thursday")]
        public int? ThursdayId { get; set; }
        public AvailabilityDay Thursday { get; set; }

        [ForeignKey("Friday")]
        public int? FridayId { get; set; }
        public AvailabilityDay Friday { get; set; }

        [ForeignKey("Saturday")]
        public int? SaturdayId { get; set; }
        public AvailabilityDay Saturday { get; set; }

        [ForeignKey("Sunday")]
        public int? SundayId { get; set; }
        public AvailabilityDay Sunday { get; set; }

    }
}