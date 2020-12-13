using PartialCrud.Data;
using System;

namespace PartialCrud.Models
{
    public class EmployeeModel
    {
        public Int64 EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public Nullable<int> Salary { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        public string Status { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CityId { get; set; }

        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
    }
}