using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.TaskAssignments
{
    public class GetTaskAssignmentsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public Guid? DocId { get; set; }
        public string? Url { get; set; }
        public DateTime? DueDateMin { get; set; }
        public DateTime? DueDateMax { get; set; }
        public string? Priority { get; set; }
        public string? Comment { get; set; }
        public Guid? AssignedUserId { get; set; }

        public GetTaskAssignmentsInput()
        {

        }
    }
}