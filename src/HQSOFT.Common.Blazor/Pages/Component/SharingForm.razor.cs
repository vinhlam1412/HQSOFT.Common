using Blazorise;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.TaskAssignments;
//using HQSOFT.Common.ShareWiths;
using HQSOFT.Common.Shared;
using HQSOFT.Common.ShareWiths;
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
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using static MudBlazor.CategoryTypes;
using System.Security.Policy;

namespace HQSOFT.Common.Blazor.Pages.Component
{
    public partial class SharingForm
    {
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int MaxCount { get; } = 1000;
        private IEnumerable<IdentityUserDto> UserList { get; set; } 
        private TaskAssignmentCreateDto TaskAssignment { get; set; } = new TaskAssignmentCreateDto();
        private IReadOnlyList<object> SelectedShareWiths { get; set; } = new List<ShareWithDto>();
        private IGrid GridShareWith { get; set; } //Id of ShareWith grid control name
        private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
        string FocusedColumn { get; set; }
        private EditContext _gridShareWithEditContext { get; set; } //Injected Editcontext of ShareWith grid

        [Parameter]
        public EventCallback<List<ShareWithDto>> OnShareWithAdding { get; set; }

        [Parameter]
        public EventCallback<NotificationDto> OnShareWithDeleting { get; set; }

        [Parameter]
        public List<ShareWithDto> EditingShareWithList { get; set; }
        
        [Parameter]
        public Guid DocId { get; set; }
        
        [Parameter]
        public string Url { get; set; }

        [Parameter]
        public string AuthServerUrl { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if(EditingShareWithList == null)
            {
                EditingShareWithList = new List<ShareWithDto>();
            }

            await GetUserListAsync();

            await InvokeAsync(StateHasChanged);

            await base.OnInitializedAsync();
        }

        private async Task GetUserListAsync()
        {
            var input = new GetIdentityUsersInput();
            input.MaxResultCount = MaxCount;
            
            var result = await IdentityUserAppService.GetListAsync(input);
            UserList = (List<IdentityUserDto>)result.Items;
        }
        private async Task<bool> IsUserExisting(Guid userId)
        {
            var result = EditingShareWithList.Find(item => (item.SharedToUserId == userId));

            if (result != null)
                return true;
            else
                return false;
        }
        private async Task AddSharing_Click()
        {
            await GridShareWith.SaveChangesAsync();
            await OnShareWithAdding.InvokeAsync(EditingShareWithList);  
        }

        private async Task DeleteShareWith()
        {
            if (SelectedShareWiths != null)
            {
                foreach (ShareWithDto row in SelectedShareWiths)
                {
                    var result = await NotificationsAppService.GetListAsync(new GetNotificationsInput { MaxResultCount = 1, DocId = DocId, Url = Url, ToUserId = row.SharedToUserId, Type = NotificationsType.Share });
                    

                    await ShareWithsAppService.DeleteAsync(row.Id);
                    EditingShareWithList.Remove(row);
                    if (result.Items.Count > 0)
                    {
                        var notifiedRecord = (List<NotificationDto>)result.Items;
                        await NotificationsAppService.DeleteAsync(notifiedRecord.FirstOrDefault().Id);
                        await InvokeAsync(async() =>
                        {
                            await OnShareWithDeleting.InvokeAsync(notifiedRecord.FirstOrDefault());
                        });
                    }                      
                }
            }
            //await InvokeAsync(StateHasChanged);
            await InvokeAsync(() =>
            {
                GridShareWith.Reload();
            });
        }

        EditContext GridShareWithEditContext
        {
            get { return GridShareWith.IsEditing() ? _gridShareWithEditContext : null; }
            set { _gridShareWithEditContext = value; }
        }
        private async Task GridShareWith_OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            if (GridShareWith.IsEditing() && GridShareWithEditContext.IsModified())
            {
                await GridShareWith.SaveChangesAsync();
                IsDataEntryChanged = true;
            }
            else
                await GridShareWith.CancelEditAsync();
        }

        private void GridShareWith_OnRowClick(GridRowClickEventArgs e)
        {
            FocusedColumn = e.Column.Name;
        }

        private async Task GridShareWith_OnRowDoubleClick(GridRowClickEventArgs e)
        {
            FocusedColumn = e.Column.Name;
            await e.Grid.StartEditRowAsync(e.VisibleIndex);
        }

        private async void GridShareWith_EditModelSaving(GridEditModelSavingEventArgs e)
        {           
            ShareWithDto editModel = (ShareWithDto)e.EditModel;
            ShareWithDto dataItem = e.IsNew ? new ShareWithDto() : EditingShareWithList.Find(item => item.SharedToUserId == editModel.SharedToUserId);
            var checkUserExisting = await IsUserExisting(editModel.SharedToUserId);
            if (!checkUserExisting & editModel.SharedToUserId != Guid.Empty)
            {
                if (editModel != null && !e.IsNew)           
                {
                    editModel.IsChanged = true;
                    IsDataEntryChanged = true;
                    EditingShareWithList.Remove(dataItem);
                    EditingShareWithList.Add(editModel);
                }
                if (editModel != null && e.IsNew)
                {
                    EditingShareWithList.Add(editModel);
                }
            }               
        }
        private async Task GridShareWith_OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "F2")
                await GridShareWith.StartEditRowAsync(GridShareWith.GetFocusedRowIndex());
        }


        private async Task BtnAdd_GridShareWith_OnClick()
        {
            await GridShareWith.SaveChangesAsync();
            await GridShareWith.StartEditNewRowAsync();
        }

        private async Task BtnDelete_GridShareWith_OnClick()
        {
            await DeleteShareWith();
        }
    }
}
