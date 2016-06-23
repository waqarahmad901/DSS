using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSSCIO.Areas.Test.Models
{
    public class ProjectResources
    {
        public int ProjectId { get; set; }
        public int ResourceId { get; set; }
        public string Status { get; set; }
        public DateTime AppoinmentDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime ActualReturnDate { get; set; }

    }
}