using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoxAndHounds.Models
{
    public class RequestOff
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Shift")]
        public int? ShiftId { get; set; }
        public Shift Shift { get; set; }
        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public bool EmployeeAccepted { get; set; }
        public bool ManagerAccepted { get; set; }
    }
}