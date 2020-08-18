using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartialCrud.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public Nullable<int> Salary { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        public string Status { get; set; }
    }
}