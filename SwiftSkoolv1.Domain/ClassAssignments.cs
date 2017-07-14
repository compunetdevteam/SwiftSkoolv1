using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HopeAcademySMS.Models
{
    public class ClassAssignments
    {
        public int ClassAssignmentId { get; set; }
        public List<string> Subjects { get; set; }
        public bool DoneAssigment { get; set; }
        public bool NotDoneAssign { get; set; }

    
}
}