using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.TaskAssignments
{
    public class TaskAssignment : AuditedAggregateRoot<Guid>
    {
        public virtual Guid DocId { get; set; }

        [NotNull]
        public virtual string Url { get; set; }

        public virtual DateTime DueDate { get; set; }

        [CanBeNull]
        public virtual string? Priority { get; set; }

        [CanBeNull]
        public virtual string? Comment { get; set; }

        public virtual Guid AssignedUserId { get; set; }

        public TaskAssignment()
        {

        }

        public TaskAssignment(Guid id, Guid docId, string url, DateTime dueDate, string priority, string comment, Guid assignedUserId)
        {

            Id = id;
            Check.NotNull(url, nameof(url));
            DocId = docId;
            Url = url;
            DueDate = dueDate;
            Priority = priority;
            Comment = comment;
            AssignedUserId = assignedUserId;
        }

    }
}