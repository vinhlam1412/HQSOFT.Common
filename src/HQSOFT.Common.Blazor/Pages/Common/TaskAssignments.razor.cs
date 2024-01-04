using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using HQSOFT.Common.TaskAssignments;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Blazor.Pages.Common
{
    public partial class TaskAssignments
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<TaskAssignmentDto> TaskAssignmentList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateTaskAssignment { get; set; }
        private bool CanEditTaskAssignment { get; set; }
        private bool CanDeleteTaskAssignment { get; set; }
        private TaskAssignmentCreateDto NewTaskAssignment { get; set; }
        private Validations NewTaskAssignmentValidations { get; set; } = new();
        private TaskAssignmentUpdateDto EditingTaskAssignment { get; set; }
        private Validations EditingTaskAssignmentValidations { get; set; } = new();
        private Guid EditingTaskAssignmentId { get; set; }
        private Modal CreateTaskAssignmentModal { get; set; } = new();
        private Modal EditTaskAssignmentModal { get; set; } = new();
        private GetTaskAssignmentsInput Filter { get; set; }
        private DataGridEntityActionsColumn<TaskAssignmentDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "taskAssignment-create-tab";
        protected string SelectedEditTab = "taskAssignment-edit-tab";
        
        public TaskAssignments()
        {
            NewTaskAssignment = new TaskAssignmentCreateDto();
            EditingTaskAssignment = new TaskAssignmentUpdateDto();
            Filter = new GetTaskAssignmentsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            TaskAssignmentList = new List<TaskAssignmentDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:TaskAssignments"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewTaskAssignment"], async () =>
            {
                await OpenCreateTaskAssignmentModalAsync();
            }, IconName.Add, requiredPolicyName: CommonPermissions.TaskAssignments.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateTaskAssignment = await AuthorizationService
                .IsGrantedAsync(CommonPermissions.TaskAssignments.Create);
            CanEditTaskAssignment = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.TaskAssignments.Edit);
            CanDeleteTaskAssignment = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.TaskAssignments.Delete);
        }

        private async Task GetTaskAssignmentsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await TaskAssignmentsAppService.GetListAsync(Filter);
            TaskAssignmentList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetTaskAssignmentsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await TaskAssignmentsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Common") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/common/task-assignments/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<TaskAssignmentDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetTaskAssignmentsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateTaskAssignmentModalAsync()
        {
            NewTaskAssignment = new TaskAssignmentCreateDto{
                DueDate = DateTime.Now,

                
            };
            await NewTaskAssignmentValidations.ClearAll();
            await CreateTaskAssignmentModal.Show();
        }

        private async Task CloseCreateTaskAssignmentModalAsync()
        {
            NewTaskAssignment = new TaskAssignmentCreateDto{
                DueDate = DateTime.Now,

                
            };
            await CreateTaskAssignmentModal.Hide();
        }

        private async Task OpenEditTaskAssignmentModalAsync(TaskAssignmentDto input)
        {
            var taskAssignment = await TaskAssignmentsAppService.GetAsync(input.Id);
            
            EditingTaskAssignmentId = taskAssignment.Id;
            EditingTaskAssignment = ObjectMapper.Map<TaskAssignmentDto, TaskAssignmentUpdateDto>(taskAssignment);
            await EditingTaskAssignmentValidations.ClearAll();
            await EditTaskAssignmentModal.Show();
        }

        private async Task DeleteTaskAssignmentAsync(TaskAssignmentDto input)
        {
            await TaskAssignmentsAppService.DeleteAsync(input.Id);
            await GetTaskAssignmentsAsync();
        }

        private async Task CreateTaskAssignmentAsync()
        {
            try
            {
                if (await NewTaskAssignmentValidations.ValidateAll() == false)
                {
                    return;
                }

                await TaskAssignmentsAppService.CreateAsync(NewTaskAssignment);
                await GetTaskAssignmentsAsync();
                await CloseCreateTaskAssignmentModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditTaskAssignmentModalAsync()
        {
            await EditTaskAssignmentModal.Hide();
        }

        private async Task UpdateTaskAssignmentAsync()
        {
            try
            {
                if (await EditingTaskAssignmentValidations.ValidateAll() == false)
                {
                    return;
                }

                await TaskAssignmentsAppService.UpdateAsync(EditingTaskAssignmentId, EditingTaskAssignment);
                await GetTaskAssignmentsAsync();
                await EditTaskAssignmentModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }
        

    }
}
