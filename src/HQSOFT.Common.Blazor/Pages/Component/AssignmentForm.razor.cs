using Blazorise;
//using HQSOFT.Common.Notifications;
using HQSOFT.Common.TaskAssignments;
//using HQSOFT.Common.ShareWiths;
using HQSOFT.Common.Shared; 
using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp;
using Abp.Extensions;
using DevExpress.Utils.Serializing;

namespace HQSOFT.Common.Blazor.Pages.Component
{
    public partial class AssignmentForm
    {
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int MaxCount { get; } = 1000;
        private IEnumerable<IdentityUserDto> UserList { get; set; } 
        private IEnumerable<IdentityUserDto> SelectedUsers { get; set; } 
        private IReadOnlyList<TaskAssignmentPriorityList> PriorityCollection { get; set; }

        private TaskAssignmentCreateDto TaskAssignment { get; set; } = new TaskAssignmentCreateDto();

        [Parameter]
        public EventCallback<TaskAssignmentData> OnTaskAssignmentAdding { get; set; }

        [Parameter]
        public IEnumerable<IdentityUserDto> AssignedUsers { get; set; }
        [Parameter]
        public string AuthServerUrl { get; set; }
        protected override async Task OnInitializedAsync()
        {   
            await GetPriorityCollectionAsync();
            await GetUserListAsync();
            
            TaskAssignment.DueDate = DateTime.Today.ToDateTimeUnspecified(); 
            await InvokeAsync(StateHasChanged); 
            await base.OnInitializedAsync();
        }
         
        private async Task GetPriorityCollectionAsync()
        {
            PriorityCollection = Enum.GetValues(typeof(TaskAssignmentPriorities))
            .OfType<TaskAssignmentPriorities>()
            .Select(t => new TaskAssignmentPriorityList()
            {
                Value = t.ToString(),
                DisplayName = L["TaskAssignment." + t.ToString()],
            }).ToList();
        }

        private async Task GetUserListAsync()
        {
            var input = new GetIdentityUsersInput();
            input.MaxResultCount = MaxCount;
            
            var result = await IdentityUserAppService.GetListAsync(input);
            UserList = (List<IdentityUserDto>)result.Items;

            var selectUsers = new List<IdentityUserDto>();
            //SelectedUsers = UserList;
            foreach (var item in AssignedUsers)
            {
                selectUsers.Add(UserList.FirstOrDefault(u => u.Id == item.Id));
            }    
            SelectedUsers = selectUsers;
        }

        private async Task AddAssigment_Click()
        {
            TaskAssignmentData taskAssigmentData = new TaskAssignmentData(
                TaskAssignment = TaskAssignment,
                SelectedUsers = SelectedUsers);
            await OnTaskAssignmentAdding.InvokeAsync(taskAssigmentData);
        }
    }
}
