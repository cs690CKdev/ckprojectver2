//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ck_project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public address()
        {
            this.branches = new HashSet<branch>();
            this.customers = new HashSet<customer>();
        }
    
        public int address_number { get; set; }
        public string address_type { get; set; }
        [Required(ErrorMessage = "The address is required")]
        [MinLength(2, ErrorMessage = "The address is not complete")]
        [MaxLength(49, ErrorMessage = "The address  must be less than 50 characters")]
        public string address1 { get; set; }
        [Required(ErrorMessage = "The city is required")]
        [MinLength(2, ErrorMessage = "The city is too short")]
        [MaxLength(49, ErrorMessage = "The city must be less than 50 characters")]
        public string city { get; set; }
        public string state { get; set; }
        [Required(ErrorMessage = "The county is required")]
        [MinLength(2, ErrorMessage = "The county is too short")]
        [MaxLength(49, ErrorMessage = "The county must be less than 50 characters")]
        public string county { get; set; }
        [Required(ErrorMessage = "The zipcode is required")]
        [MinLength(5, ErrorMessage = "The zipcode must be 5 numbers")]
        [MaxLength(5, ErrorMessage = "The zipcode must be 5 numbers")]
        [Range(00001, 99999, ErrorMessage = "The zipcode must be 5 numbers")]
        public string zipcode { get; set; }
        public bool deleted { get; set; }
        public Nullable<int> lead_number { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<branch> branches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customer> customers { get; set; }
    }
}