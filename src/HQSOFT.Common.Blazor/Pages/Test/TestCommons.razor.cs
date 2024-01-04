using AutoMapper.Internal.Mappers;
using Blazorise;
using DevExpress.Blazor;
using HQSOFT.Common.TestCommons;
using HQSOFT.Common.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using HQSOFT.Common.Blazor.Pages.Component;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using HQSOFT.Common.Comments;
using System.Net;

namespace HQSOFT.Common.Blazor.Pages.Test
{
	public partial class TestCommons
	{
		protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
		protected PageToolbar Toolbar { get; } = new PageToolbar();
		private HubConnection _hubConnection;

		private int PageSize { get; } = 50;
		private int MaxCount { get; } = 1000;
		private bool CanCreateTestCommon { get; set; }
		private bool CanEditTestCommon { get; set; }
		private bool CanDeleteTestCommon { get; set; }
		private HQSOFTFormActivity formActivity;
		private TestCommonUpdateDto EditingTestCommon { get; set; } = new TestCommonUpdateDto();  //Editing row on grid
		private Guid EditingTestCommonId { get; set; } = Guid.Parse("3a0f6f5c-3af1-e54c-cb22-71e2e59d9e44"); //Editing TestCommon Id on 

		private List<Guid> DocIds;
		private List<TestCommonUpdateDto> TestCommonList { get; set; } = new List<TestCommonUpdateDto>(); //Data source used to bind to grid
		private IReadOnlyList<object> SelectedTestCommons { get; set; } = new List<TestCommonUpdateDto>(); //Selected rows on grid
		private GetTestCommonsInput Filter { get; set; } //Used for Search box
		private IGrid GridTestCommon { get; set; } //Id of TestCommon grid control name
		private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
		private bool ShowInteractionForm { get; set; } = true;
		string FocusedColumn { get; set; }
		private EditContext _gridTestCommonEditContext { get; set; } //Injected Editcontext of TestCommon grid

		private readonly IUiMessageService _uiMessageService; //Injected UIMessage

		//==================================Initialize Section===================================
		#region
		public TestCommons(IUiMessageService uiMessageService)
		{
			Filter = new GetTestCommonsInput
			{
				MaxResultCount = MaxCount,
			};
			_uiMessageService = uiMessageService;

			//int maxIdx = TestCommonList.Max(x => x.Idx);
			//int missingNumber = MissingNumberHelper.FindMissingNumber(TestCommonList, x => x.Idx, maxIdx);
		}

		protected override async Task OnInitializedAsync()
		{
			DocIds = new List<Guid>(); // Initialize DocIds list
			DocIds.Clear();
			await SetPermissionsAsync();
			await SetBreadcrumbItemsAsync();
			await SetToolbarItemsAsync();
		}

		public async Task GetHistoriesAsync()
		{
			await InvokeAsync(StateHasChanged);
			await formActivity.GetEntityChangeAsync();
			await formActivity.GetHistoryListAsync();
		}

		public async Task GetConnectSignalR()
		{
			var apiURL = Configuration.GetValue<string>("SignalR:HistoryUrl");
			try
			{
				_hubConnection = new HubConnectionBuilder()
							 .WithUrl($"{apiURL}", options =>
							 {
								 if (HttpContextAccessor.HttpContext != null)
								 {
									 foreach (var cookie in HttpContextAccessor.HttpContext.Request.Cookies)
									 {
										 options.Cookies.Add(new Cookie(cookie.Key, cookie.Value, null, "localhost"));
									 }
								 }
							 }).Build();
				await _hubConnection.StartAsync();
				Console.WriteLine("SignalR connected");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error starting HubConnection: " + ex.Message);
			}
		}
		Task SendHistory() => _hubConnection.SendAsync("SendHistory");
		public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

		public void Dispose()
		{
			_ = _hubConnection.DisposeAsync();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			base.OnAfterRender(firstRender);
			await JSRuntime.InvokeVoidAsync("AssignGotFocus");
		}

		private async Task OnBeforeInternalNavigation(LocationChangingContext context)
		{
			bool checkSaving = await SavingConfirmAsync();
			if (!checkSaving)
				context.PreventNavigation();
		}

		private async Task SetPermissionsAsync()
		{
			CanCreateTestCommon = await AuthorizationService.IsGrantedAsync(CommonPermissions.TestCommons.Create);
			CanEditTestCommon = await AuthorizationService.IsGrantedAsync(CommonPermissions.TestCommons.Edit);
			CanDeleteTestCommon = await AuthorizationService.IsGrantedAsync(CommonPermissions.TestCommons.Delete);

		}

		protected virtual ValueTask SetBreadcrumbItemsAsync()
		{
			BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:TestCommons"]));
			return ValueTask.CompletedTask;
		}

		protected virtual ValueTask SetToolbarItemsAsync()
		{
			Toolbar.AddButton(L["Refresh"], async () =>
			{
				bool checkSaving = await SavingConfirmAsync();
				if (!checkSaving)
					await GetTestCommonsAsync();
			},
			icon: "fa fa-sync",
			Color.Primary);

			Toolbar.AddButton(L["Save"], SaveTestCommonAsync,
			IconName.Save,
			Color.Primary,
			requiredPolicyName: CommonPermissions.TestCommons.Edit, disabled: !CanEditTestCommon);

			Toolbar.AddButton(L["Delete"], DeleteTestCommon,
			IconName.Delete,
			Color.Danger,
			requiredPolicyName: CommonPermissions.TestCommons.Delete, disabled: !CanDeleteTestCommon);

			Toolbar.AddButton("", ToggleInteractionFormAsync,
			icon: "fa fa-bars",
			Color.Secondary);

			return ValueTask.CompletedTask;
		}
		#endregion

		//======================Load Data Source for ListView & Others===========================
		#region

		#endregion

		//======================CRUD & Load Main Data Source Section=============================
		#region
		private List<Guid> CommentIds; // Danh sách ID của Comment
		private List<Guid> TestCommonIds; // Danh sách ID của TestCommon 


		private GetCommentsInput FilterCommentList { get; set; } = new GetCommentsInput();
		private IReadOnlyList<CommentDto> commentList { get; set; } = new List<CommentDto>();
		public async Task GetCommentListAsync()
		{
			FilterCommentList.MaxResultCount = MaxCount;

			var result = await CommentsAppService.GetListAsync(FilterCommentList);
			commentList = result.Items.OrderByDescending(m => m.CreationTime).ToList();
			CommentIds = commentList.Select(x => x.Id).ToList(); // Lấy danh sách ID của Comment
			DocIds.AddRange(commentList.Select(x => x.DocId)); // Thêm các DocId từ commentsList vào danh sách DocIds
			StateHasChanged();
		}

		private async Task GetTestCommonsAsync()
		{
			if (GridTestCommon != null && GridTestCommon.IsEditing())
				await GridTestCommon.CancelEditAsync();
			Filter.MaxResultCount = MaxCount;
			var result = await TestCommonsAppService.GetListAsync(Filter);

			TestCommonList = ObjectMapper.Map<List<TestCommonDto>, List<TestCommonUpdateDto>>((List<TestCommonDto>)result.Items);
			GridTestCommon.Reload();
			IsDataEntryChanged = false;
			await InvokeAsync(StateHasChanged);
		}
		public async Task<List<Guid>> GetDocIdsFromTestCommonsAsync()
		{
			var docIds = new List<Guid>();

			if (GridTestCommon != null && GridTestCommon.IsEditing())
				await GridTestCommon.CancelEditAsync();
			Filter.MaxResultCount = MaxCount;
			var result = await TestCommonsAppService.GetListAsync(Filter);

			TestCommonList = ObjectMapper.Map<List<TestCommonDto>, List<TestCommonUpdateDto>>((List<TestCommonDto>)result.Items);
			GridTestCommon.Reload();
			IsDataEntryChanged = false;

			foreach (var testCommon in TestCommonList)
			{
				var docId = testCommon.Id; // Giả sử DocId là một thuộc tính trong đối tượng TestCommon
				docIds.Add(docId);
			}

			return docIds;
		}

		public async Task<List<Guid>> GetDocIdsFromCommentsAsync()
		{
			DocIds.Clear();
			var docIds = new List<Guid>();

			FilterCommentList.MaxResultCount = PageSize;

			var result = await CommentsAppService.GetListAsync(FilterCommentList);
			commentList = result.Items.OrderByDescending(m => m.CreationTime).ToList();

			foreach (var comment in commentList)
			{
				var docId = comment.DocId; // Giả sử DocId là một thuộc tính trong đối tượng Comment
				docIds.Add(docId);
			}

			return docIds;
		}


		//public async Task<List<Guid>> GetDocIdsFromTestCommonsAsync()
		//{
		//	var docIds = new List<Guid>();

		//	if (GridTestCommon != null && GridTestCommon.IsEditing())
		//		await GridTestCommon.CancelEditAsync();
		//	Filter.MaxResultCount = MaxCount;
		//	var result = await TestCommonsAppService.GetListAsync(Filter);

		//	TestCommonList = ObjectMapper.Map<List<TestCommonDto>, List<TestCommonUpdateDto>>((List<TestCommonDto>)result.Items);
		//	GridTestCommon.Reload();
		//	IsDataEntryChanged = false;

		//	foreach (var testCommon in TestCommonList)
		//	{
		//		var docId = testCommon.Id; // Giả sử DocId là một thuộc tính trong đối tượng TestCommon
		//		docIds.Add(docId);
		//	}

		//	return docIds;
		//}

		//public async Task GetEntityChangeForCommentsAsync()
		//{
		//	var docIds = await GetDocIdsFromTestCommonsAsync();

		//	DocIds.AddRange(docIds);

		//	await  formActivity.GetEntityChangeAsync();
		//	await  formActivity.GetHistoryListAsync();
		//}

		private async Task SaveTestCommonAsync()
		{
			try
			{
				await GridTestCommon.SaveChangesAsync();
				if (IsDataEntryChanged)
				{
					foreach (var TestCommon in TestCommonList)
					{
						if (TestCommon.ConcurrencyStamp == string.Empty)
							await TestCommonsAppService.CreateAsync(ObjectMapper.Map<TestCommonUpdateDto, TestCommonCreateDto>(TestCommon));
						else if (TestCommon.IsChanged)
							await TestCommonsAppService.UpdateAsync(TestCommon.Id, TestCommon);
					}
					await GetTestCommonsAsync();
					await InvokeAsync(StateHasChanged);
				}
			}
			catch (Exception ex)
			{
				await HandleErrorAsync(ex);
			}
		}
		private async Task DeleteTestCommon()
		{
			var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
			if (confirmed)
			{
				if (SelectedTestCommons != null)
				{
					foreach (TestCommonUpdateDto row in SelectedTestCommons)
					{
						await TestCommonsAppService.DeleteAsync(row.Id);
						TestCommonList.Remove(row);
					}
				}
				GridTestCommon.Reload();
				await InvokeAsync(StateHasChanged);
			}
		}

		#endregion
		//=====================================Validations=======================================
		#region
		private async Task<bool> SavingConfirmAsync()
		{
			if (IsDataEntryChanged)
			{
				var confirmed = await _uiMessageService.Confirm(L["SavingConfirmationMessage"]);
				if (confirmed)
					return true;
				else
					return false;
			}
			else
				return true;
		}
		#endregion

		//============================Controls triggers/events===================================
		#region

		EditContext GridTestCommonEditContext
		{
			get { return GridTestCommon.IsEditing() ? _gridTestCommonEditContext : null; }
			set { _gridTestCommonEditContext = value; }
		}
		private async Task GridTestCommon_OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
		{
			if (GridTestCommon.IsEditing() && GridTestCommonEditContext.IsModified())
			{
				await GridTestCommon.SaveChangesAsync();
				IsDataEntryChanged = true;
			}
			else
				await GridTestCommon.CancelEditAsync();
		}

		private void GridTestCommon_OnRowClick(GridRowClickEventArgs e)
		{
			FocusedColumn = e.Column.Name;
		}

		private async Task GridTestCommon_OnRowDoubleClick(GridRowClickEventArgs e)
		{
			FocusedColumn = e.Column.Name;
			if (CanEditTestCommon)
			{
				await e.Grid.StartEditRowAsync(e.VisibleIndex);
				EditingTestCommon = (TestCommonUpdateDto)e.Grid.GetFocusedDataItem();
				EditingTestCommonId = EditingTestCommon.Id;
			}
		}
		private void GridTestCommon_OnCustomizeEditModel(GridCustomizeEditModelEventArgs e)
		{
			if (e.IsNew)
			{
				var newRow = (TestCommonUpdateDto)e.EditModel;
				newRow.Id = Guid.Empty;
				newRow.ConcurrencyStamp = string.Empty;
				if (GridTestCommon.GetVisibleRowCount() > 0)
					newRow.Idx = TestCommonList.Max(x => x.Idx) + 1;
				else
					newRow.Idx = 1;
			}
			else
			{
				EditingTestCommon = (TestCommonUpdateDto)e.Grid.GetFocusedDataItem();
				EditingTestCommonId = EditingTestCommon.Id;
			}
		}

		private void GridTestCommon_EditModelSaving(GridEditModelSavingEventArgs e)
		{
			TestCommonUpdateDto editModel = (TestCommonUpdateDto)e.EditModel;
			TestCommonUpdateDto dataItem = e.IsNew ? new TestCommonUpdateDto() : TestCommonList.Find(item => item.Idx == editModel.Idx);

			if (editModel != null && !e.IsNew)
			{
				editModel.IsChanged = true;
				IsDataEntryChanged = true;
				TestCommonList.Remove(dataItem);
				TestCommonList.Add(editModel);
			}
			if (editModel != null && e.IsNew)
			{
				editModel.IsChanged = true;
				IsDataEntryChanged = true;
				TestCommonList.Add(editModel);
			}
		}
		private async Task GridTestCommon_OnKeyDown(KeyboardEventArgs e)
		{
			if (e.Key == "F2")
				await GridTestCommon.StartEditRowAsync(GridTestCommon.GetFocusedRowIndex());
		}

		void GridTestCommon_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
		{
			((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
		}

		private async Task BtnAdd_GridTestCommon_OnClick()
		{
			await GridTestCommon.SaveChangesAsync();
			await GridTestCommon.StartEditNewRowAsync();
		}

		private async Task BtnDelete_GridTestCommon_OnClick()
		{
			await DeleteTestCommon();
		}

		private async Task ToggleInteractionFormAsync()
		{
			ShowInteractionForm = !ShowInteractionForm;
		}

		#endregion

		//============================Application Functions======================================
		#region

		private bool loadButtonActive = false;

		private async Task HandleCommentAdded()
		{
			await formActivity.GetCommentListAsync();
		}

		#endregion
	}
}