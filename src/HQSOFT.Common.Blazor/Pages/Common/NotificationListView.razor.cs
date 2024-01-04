using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Blazorise;
using Blazorise.Snackbar;
using Blazorise.DataGrid;

using Microsoft.AspNetCore.Authorization;

using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

using HQSOFT.Common.Blazor.Pages.Component.Toolbar;
using Microsoft.AspNetCore.Components;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.Permissions;

namespace HQSOFT.Common.Blazor.Pages.Common
{
	public partial class NotificationListView
	{
		protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
		protected PageToolbar Toolbar { get; } = new PageToolbar();
		protected bool ShowAdvancedFilters { get; set; }
		private int PageSize { get; set; } = LimitedResultRequestDto.DefaultMaxResultCount;
		private int CurrentPage { get; set; } = 1;
		private string CurrentSorting { get; set; } = string.Empty;
		private int TotalCount { get; set; }

		private bool CanCreateNotification { get; set; }
		private bool CanEditNotification { get; set; }
		private bool CanDeleteNotification { get; set; }
		private Guid EditingNotificationId { get; set; }

		private GetNotificationsInput Filter { get; set; }
		private IReadOnlyList<NotificationsTypeList> NotificationsTypeList { get; set; } = new List<NotificationsTypeList>();
		private List<NotificationDto> SelectedNotifications { get; set; } = new List<NotificationDto>();
		private IReadOnlyList<NotificationDto> NotificationList { get; set; } = new List<NotificationDto>();

		private readonly IUiMessageService _uiMessageService;
		SnackbarStack _uiNotification;


		//==================================Initialize Section===================================
		public NotificationListView(IUiMessageService uiMessageService)
		{
			Filter = new GetNotificationsInput
			{
				MaxResultCount = PageSize,
				SkipCount = (CurrentPage - 1) * PageSize,
				Sorting = CurrentSorting
			};
			_uiMessageService = uiMessageService;
		}

		protected override async Task OnInitializedAsync()
		{
			await SetPermissionsAsync();
			await SetToolbarItemsAsync();
			await SetBreadcrumbItemsAsync();
			await GetNotificationTypeLookupAsync();
			await GetClassDataAsync();
		}

		private async Task GetNotificationTypeLookupAsync()
		{
			NotificationsTypeList = Enum.GetValues(typeof(NotificationsType))
			.OfType<NotificationsType>()
			.Select(t => new NotificationsTypeList()
			{
				Value = t.ToString(),
				DisplayName = t.ToString(),

			}).ToList();
		}

		protected virtual ValueTask SetBreadcrumbItemsAsync()
		{
			BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Notifications"]));
			return ValueTask.CompletedTask;
		}

		protected virtual ValueTask SetToolbarItemsAsync()
		{
			Toolbar.AddButton(L["Refresh"], async () => await GetClassDataAsync(true),
			icon: "fa fa-sync",
			Color.Primary);

			Toolbar.AddComponent<ListViewHorizontal>();

			var parmAction2 = new Dictionary<string, object>()
				{
					{"DownloadAsExcelAsync", EventCallback.Factory.Create(this, DownloadAsExcelAsync) },
					{"Delete", EventCallback.Factory.Create(this, Delete)}
				};
			Toolbar.AddComponent<ListViewAction>(parmAction2);
			return ValueTask.CompletedTask;
		}

		private async Task SetPermissionsAsync()
		{
			CanCreateNotification = await AuthorizationService
				.IsGrantedAsync(CommonPermissions.Notifications.Create);
			CanEditNotification = await AuthorizationService
				.IsGrantedAsync(CommonPermissions.Notifications.Edit);
			CanDeleteNotification = await AuthorizationService
							.IsGrantedAsync(CommonPermissions.Notifications.Delete);
		}


		//======================Load Data Source for ListView & Others=========================== 
		private async Task GetClassDataAsync(bool isRefresh)
		{
			if (isRefresh)
			{
				Filter = new GetNotificationsInput() { ToUserId = CurrentUser.Id }; // Clear all filter values for refresh
			}
			else
			{
				Filter.MaxResultCount = PageSize;
				Filter.SkipCount = (CurrentPage - 1) * PageSize;
				Filter.Sorting = CurrentSorting;
				Filter.ToUserId = CurrentUser.Id;
			}

			var result = await NotificationsAppService.GetListAsync(Filter);
			NotificationList = result.Items;
			TotalCount = (int)result.TotalCount;
		}

		private async Task GetClassDataAsync()
		{
			Filter.MaxResultCount = PageSize;
			Filter.SkipCount = (CurrentPage - 1) * PageSize;
			Filter.Sorting = CurrentSorting;
			Filter.ToUserId = CurrentUser.Id;

			var result = await NotificationsAppService.GetListAsync(Filter);
			NotificationList = result.Items;
			TotalCount = (int)result.TotalCount;
		}

		private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<NotificationDto> e)
		{
			CurrentSorting = e.Columns
				.Where(c => c.SortDirection != SortDirection.Default)
				.Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
				.JoinAsString(",");
			CurrentPage = e.Page;
			await GetClassDataAsync();
			await InvokeAsync(StateHasChanged);
		}

		private async Task Delete()
		{
			if (SelectedNotifications.Count > 0)
			{
				var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
				if (confirmed)
				{
					foreach (var selectedserClass in SelectedNotifications)
					{
						await NotificationsAppService.DeleteAsync(selectedserClass.Id);
					}
					await _uiNotification.PushAsync(
							L["Notification:Delete"], SnackbarColor.Danger
						);
					await GetClassDataAsync();
				}
			}
		}

		//============================Controls triggers/events===================================
		#region 
		private async Task DownloadAsExcelAsync()
		{
			//var token = (await NotificationsAppService.GetDownloadTokenAsync()).Token;
			//var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Inventory") ??
			//await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
			//NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/core-backend/a-bCCodes/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
		}

		private async Task ChangePageSize(int value)
		{
			PageSize = value;
			await GetClassDataAsync();
		}

		public static string TruncateText(string text, int maxLength) // Cắt chuỗi
		{
			if (text.Length <= maxLength)
				return text;

			return text.Substring(0, maxLength) + "...";
		}

		public static string TruncateTextId(string text, int maxLength) // Cắt chuỗi
		{
			if (text.Length <= maxLength)
				return text;

			return text.Substring(0, maxLength);
		}

		async Task SelectedChangeRow(List<NotificationDto> e)
		{
			await ShowButtonAction();
		}
		private bool isSelected { get; set; } = false;
		private async Task ShowButtonAction()
		{
			if (SelectedNotifications.Count > 0)
			{
				ShowActionListView.UnreadCount = !isSelected;
				await InvokeAsync(StateHasChanged);
			}
			else
			{
				ShowActionListView.UnreadCount = isSelected;
				await InvokeAsync(StateHasChanged);
			}
		}

		#region Get Time 
		public string GetTimeAgo(DateTime creationTime)
		{
			TimeSpan timeDifference = DateTime.Now - creationTime;

			if (timeDifference.TotalSeconds < 60)
			{
				return $"{(int)timeDifference.TotalSeconds} s";
			}
			else if (timeDifference.TotalMinutes < 60)
			{
				return $"{(int)timeDifference.TotalMinutes} m";
			}
			else if (timeDifference.TotalHours < 24)
			{
				return $"{(int)timeDifference.TotalHours} h";
			}
			else if (timeDifference.TotalDays < 7)
			{
				return $"{(int)timeDifference.TotalDays} d";
			}
			else if (timeDifference.TotalDays < 30)
			{
				int weeks = (int)(timeDifference.TotalDays / 7);
				return $"{weeks} w";
			}
			else if (timeDifference.TotalDays < 30)
			{
				int months = (int)(timeDifference.TotalDays / 30);
				return $"{months} M";
			}
			else
			{
				int years = (int)(timeDifference.TotalDays / 365);
				return $"{years} y";
			}
		}
		#endregion
		 
		protected void GoToEditPage(NotificationDto url, NotificationDto docId)
		{ 
			NavigationManager.NavigateTo($"{url.Url}/{docId.DocId}");
		}
		#endregion
	}
}
