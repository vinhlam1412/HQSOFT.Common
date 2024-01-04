using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.TaskAssignments
{
    public class TaskAssignmentDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid DocId { get; set; }
        public string Url { get; set; }
        public DateTime DueDate { get; set; }
        public string? Priority { get; set; }
        public string? Comment { get; set; }
        public Guid AssignedUserId { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsChanged { get; set; }
    }
}