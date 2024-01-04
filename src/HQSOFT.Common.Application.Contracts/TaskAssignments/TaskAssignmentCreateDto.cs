using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.Common.TaskAssignments
{
    public class TaskAssignmentCreateDto
    {
        public Guid DocId { get; set; }
        [Required]
        public string Url { get; set; }
        public DateTime DueDate { get; set; }
        public string? Priority { get; set; }
        public string? Comment { get; set; }
        public Guid AssignedUserId { get; set; }
    }
}