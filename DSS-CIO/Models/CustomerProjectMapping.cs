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
    
    public partial class CustomerProjectMapping
    {
        public string CustomerID { get; set; }
        public int ProjectID { get; set; }
        public string SatisfactionLevel { get; set; }
        public int CustomerProjectId { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Project Project { get; set; }
    }
}