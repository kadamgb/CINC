using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NQR.CINC.Web.Models
{
    public class PatientModel
    {
        public long Id { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<long> Mobile { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<int> Weight { get; set; }
        public string Height { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public Nullable<long> AddharNumber { get; set; }
        public string Gaurdian { get; set; }
        public Nullable<long> EmergencyContact { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public long UserId { get; set; }
    }
}