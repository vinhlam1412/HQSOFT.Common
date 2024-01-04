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
using HQSOFT.Common.ShareWiths;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Blazor.Pages.Common
{
    public partial class ShareWiths
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<ShareWithDto> ShareWithList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateShareWith { get; set; }
        private bool CanEditShareWith { get; set; }
        private bool CanDeleteShareWith { get; set; }
        private ShareWithCreateDto NewShareWith { get; set; }
        private Validations NewShareWithValidations { get; set; } = new();
        private ShareWithUpdateDto EditingShareWith { get; set; }
        private Validations EditingShareWithValidations { get; set; } = new();
        private Guid EditingShareWithId { get; set; }
        private Modal CreateShareWithModal { get; set; } = new();
        private Modal EditShareWithModal { get; set; } = new();
        private GetShareWithsInput Filter { get; set; }
        private DataGridEntityActionsColumn<ShareWithDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "shareWith-create-tab";
        protected string SelectedEditTab = "shareWith-edit-tab";
        
        public ShareWiths()
        {
            NewShareWith = new ShareWithCreateDto();
            EditingShareWith = new ShareWithUpdateDto();
            Filter = new GetShareWithsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            ShareWithList = new List<ShareWithDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:ShareWiths"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            
            
            Toolbar.AddButton(L["NewShareWith"], async () =>
            {
                await OpenCreateShareWithModalAsync();
            }, IconName.Add, requiredPolicyName: CommonPermissions.ShareWiths.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateShareWith = await AuthorizationService
                .IsGrantedAsync(CommonPermissions.ShareWiths.Create);
            CanEditShareWith = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.ShareWiths.Edit);
            CanDeleteShareWith = await AuthorizationService
                            .IsGrantedAsync(CommonPermissions.ShareWiths.Delete);
        }

        private async Task GetShareWithsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await ShareWithsAppService.GetListAsync(Filter);
            ShareWithList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetShareWithsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ShareWithDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetShareWithsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateShareWithModalAsync()
        {
            NewShareWith = new ShareWithCreateDto{
                
                
            };
            await NewShareWithValidations.ClearAll();
            await CreateShareWithModal.Show();
        }

        private async Task CloseCreateShareWithModalAsync()
        {
            NewShareWith = new ShareWithCreateDto{
                
                
            };
            await CreateShareWithModal.Hide();
        }

        private async Task OpenEditShareWithModalAsync(ShareWithDto input)
        {
            var shareWith = await ShareWithsAppService.GetAsync(input.Id);
            
            EditingShareWithId = shareWith.Id;
            EditingShareWith = ObjectMapper.Map<ShareWithDto, ShareWithUpdateDto>(shareWith);
            await EditingShareWithValidations.ClearAll();
            await EditShareWithModal.Show();
        }

        private async Task DeleteShareWithAsync(ShareWithDto input)
        {
            await ShareWithsAppService.DeleteAsync(input.Id);
            await GetShareWithsAsync();
        }

        private async Task CreateShareWithAsync()
        {
            try
            {
                if (await NewShareWithValidations.ValidateAll() == false)
                {
                    return;
                }

                await ShareWithsAppService.CreateAsync(NewShareWith);
                await GetShareWithsAsync();
                await CloseCreateShareWithModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditShareWithModalAsync()
        {
            await EditShareWithModal.Hide();
        }

        private async Task UpdateShareWithAsync()
        {
            try
            {
                if (await EditingShareWithValidations.ValidateAll() == false)
                {
                    return;
                }

                await ShareWithsAppService.UpdateAsync(EditingShareWithId, EditingShareWith);
                await GetShareWithsAsync();
                await EditShareWithModal.Hide();                
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
