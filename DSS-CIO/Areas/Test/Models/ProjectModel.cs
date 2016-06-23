using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSSCIO.Areas.Test.Models
{
    public class ProjectModel
    {
        public int[] Piechart1 { get; set; }
        public int[] Piechart2 { get; set; }
        public int[] Piechart3 { get; set; }
        public int[] Piechart4 { get; set; }
        public int[] Piechart5 { get; set; }
        public int[] Piechart6 { get; set; }

        public int[] Piechart7 { get; set; }

        public int projectsCompletedToday { get; set; }
        public int projectsHavinyTodayDeadline { get; set; }
        public int projectsExcedingTodayDeadline { get; set; }


    }
}