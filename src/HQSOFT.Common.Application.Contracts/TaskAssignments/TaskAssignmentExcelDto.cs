using System;

namespace HQSOFT.Common.TaskAssignments
{
    public class TaskAssignmentExcelDto
    {
        public Guid DocId { get; set; }
        public string Url { get; set; }
        public DateTime DueDate { get; set; }
        public string? Priority { get; set; }
        public string? Comment { get; set; }
        public Guid AssignedUserId { get; set; }
    }
}