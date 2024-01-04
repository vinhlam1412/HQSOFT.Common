using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.Common
{
    public enum AssignToPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4,
    }
    public class AssignToPriorityList
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }

    }
}
