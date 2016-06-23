using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSSCIO.Areas.Test.Models
{
    public class ProjectDetailModel
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string PeopleCount { get; set; }
        public string LanchDate { get; set; }
        public string ExpectedEndDate { get; set; }
        public int TotalMileStone { get; set; }
        public string ProjStatus { get; set; }

    }
}