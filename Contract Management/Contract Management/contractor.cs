//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Contract_Management
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class contractor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public contractor()
        {
            this.applytenders = new HashSet<applytender>();
        }
        [DisplayName("User ID")]
        [Required]
        public string userid { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string fname { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string lname { get; set; }
        [Required]
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public Nullable<System.DateTime> dob { get; set; }
        [DisplayName("Gender")]
        [Required]
        public string gender { get; set; }
        [Required]
        [DisplayName("Contact Number")]
        [MinLength(10)]
        [MaxLength(10)]
        public string phone { get; set; }
        [Required]
        [DisplayName("Address")]
        public string address { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email ID")]
        public string email { get; set; }
        [Required]
        [DisplayName("Category")]
        public string category { get; set; }
        [Required]
        [DisplayName("Password")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string pwd { get; set; }
        public Nullable<int> q1 { get; set; }
        [Required]
        [DisplayName("Answer 1")]
        public string ans1 { get; set; }
        public Nullable<int> q2 { get; set; }
        [Required]
        [DisplayName("Answer 2")]
        public string ans2 { get; set; }
        public Nullable<int> q3 { get; set; }
        [Required]
        [DisplayName("Answer 3")]
        public string ans3 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<applytender> applytenders { get; set; }
        public virtual securityque securityque { get; set; }
        public virtual securityque securityque1 { get; set; }
        public virtual securityque securityque2 { get; set; }
    }
}
