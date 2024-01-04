using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.TaskAssignments
{
    public class TaskAssignmentManager : DomainService
    {
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;

        public TaskAssignmentManager(ITaskAssignmentRepository taskAssignmentRepository)
        {
            _taskAssignmentRepository = taskAssignmentRepository;
        }

        public async Task<TaskAssignment> CreateAsync(
        Guid docId, string url, DateTime dueDate, string priority, string comment, Guid assignedUserId)
        {
            Check.NotNullOrWhiteSpace(url, nameof(url));
            Check.NotNull(dueDate, nameof(dueDate));

            var taskAssignment = new TaskAssignment(
             GuidGenerator.Create(),
             docId, url, dueDate, priority, comment, assignedUserId
             );

            return await _taskAssignmentRepository.InsertAsync(taskAssignment);
        }

        public async Task<TaskAssignment> UpdateAsync(
            Guid id,
            Guid docId, string url, DateTime dueDate, string priority, string comment, Guid assignedUserId, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(url, nameof(url));
            Check.NotNull(dueDate, nameof(dueDate));

            var taskAssignment = await _taskAssignmentRepository.GetAsync(id);

            taskAssignment.DocId = docId;
            taskAssignment.Url = url;
            taskAssignment.DueDate = dueDate;
            taskAssignment.Priority = priority;
            taskAssignment.Comment = comment;
            taskAssignment.AssignedUserId = assignedUserId;

            taskAssignment.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _taskAssignmentRepository.UpdateAsync(taskAssignment);
        }

    }
}