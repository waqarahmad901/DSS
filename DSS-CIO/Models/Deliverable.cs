//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DSS_CIO.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Deliverable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deliverable()
        {
            this.Complaints = new HashSet<Complaint>();
            this.EmployeeTaskAssignments = new HashSet<EmployeeTaskAssignment>();
        }
    
        public int TaskID { get; set; }
        public string Description { get; set; }
        public string AssignedBy { get; set; }
        public Nullable<double> Version { get; set; }
        public string ReviewedBy { get; set; }
        public int MilestoneID { get; set; }
        public int ProjectID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual Milestone Milestone { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeTaskAssignment> EmployeeTaskAssignments { get; set; }
    }
}