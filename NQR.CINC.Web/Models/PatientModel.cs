using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NQR.CINC.Web.Models
{
    public class PatientModel
    {
        public long Id { get; set; }

        public string Salutation { get; set; }

        [Required(ErrorMessage = "Patient name is Require.")]
        [Display(Name = "Patient Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle name is Require.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is Require.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [Display(Name = "Mobile")]
        public Nullable<long> Mobile { get; set; }

        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date is required")]
        [DataType(DataType.Date, ErrorMessage = "Wrong date format.")]
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