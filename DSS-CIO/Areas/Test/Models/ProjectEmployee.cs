using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSSCIO.Areas.Test.Models
{
    public class ProjectEmployee
    {

        public int ProjectId { get; set; }
        public string CNIC { get; set; }
        public string Role { get; set; }
        public string performance { get; set; }
        public DateTime AppointmentDate { get; set; }

    }
}