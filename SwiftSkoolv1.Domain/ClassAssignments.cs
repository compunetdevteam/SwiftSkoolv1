﻿using System.Collections.Generic;

namespace SwiftSkoolv1.Domain
{
    public class ClassAssignments
    {
        public int ClassAssignmentId { get; set; }
        public List<string> Subjects { get; set; }
        public bool DoneAssigment { get; set; }
        public bool NotDoneAssign { get; set; }


    }
}