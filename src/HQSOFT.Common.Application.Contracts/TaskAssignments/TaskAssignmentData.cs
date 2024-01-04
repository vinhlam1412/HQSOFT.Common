using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;

namespace HQSOFT.Common.TaskAssignments
{
    public class TaskAssignmentData
    {
        public TaskAssignmentCreateDto TaskAssignment { get; set; }
        public IEnumerable<IdentityUserDto> SelectedUsers { get; set; }
        public TaskAssignmentData(TaskAssignmentCreateDto taskAssignment, IEnumerable<IdentityUserDto> selectedUsers)
        {
            TaskAssignment = taskAssignment;
            SelectedUsers = selectedUsers;
        }

        public TaskAssignmentData()
        {
            
        }
    }
    
}
