using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.Common.TaskAssignments
{
    public enum TaskAssignmentPriorities
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4,
    }
    public class TaskAssignmentPriorityList
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
    }
}
