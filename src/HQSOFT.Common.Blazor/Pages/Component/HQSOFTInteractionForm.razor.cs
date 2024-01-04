using Blazorise;
using DevExpress.Blazor;

using HQSOFT.Common.Notifications;
using HQSOFT.Common.TaskAssignments;
using HQSOFT.Common.ShareWiths;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Auditing;
using static Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup.EmailSettingGroupViewComponent;
using Volo.Abp.SettingManagement;
using System.Net;

namespace HQSOFT.Common.Blazor.Pages.Component
{
	public partial class HQSOFTInteractionForm
	{
		private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
		private int MaxCount { get; } = 1000;
		private List<IdentityUserDto> AssignedUserList { get; set; } = new List<IdentityUserDto>();
		private IEnumerable<IdentityUserDto> SelectedUsers { get; set; } = new List<IdentityUserDto>();
		private List<TaskAssignmentDto> TaskAssignmentList { get; set; } = new List<TaskAssignmentDto>();
		private List<IdentityUserDto> SharedUserList { get; set; }
		private IEnumerable<IdentityUserDto> SharedUsers { get; set; }
		private IReadOnlyList<NotificationsTypeList> NotificationsTypeList { get; set; } = new List<NotificationsTypeList>();
		private List<ShareWithDto> ShareWithList { get; set; } = new List<ShareWithDto>();
		private NotificationCreateDto notiMessage = new NotificationCreateDto();
		private List<NotificationCreateDto> notiMessages = new List<NotificationCreateDto>();
		private HubConnection hubConnection;
		private DxPopup EditAssignedToModal { get; set; }
		private DxPopup EditSharingModal { get; set; }
		private readonly IConfiguration _configuration;
		private string authServerUrl;
		private readonly IAuditingManager _auditingManager;

		[Parameter]
		public Guid DocId { get; set; }

		[Parameter]
		public string Url { get; set; }

		[Parameter]
		public string TableName { get; set; }

		[Parameter]
		public string TypeTable { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await GetAssignedToCollectionAsync();
			await GetSharedCollectionAsync();
			await GetSharedUsersAsync();
			await GetConnectSignalRAsync();
			await GetNotificationTypeLookupAsync();
			await base.OnInitializedAsync();
		}

		public HQSOFTInteractionForm(IConfiguration configuration, IAuditingManager auditingManager)
		{
			_configuration = configuration;
			_auditingManager = auditingManager;
			authServerUrl = _configuration["AuthServer:Authority"];

		}
		private async Task GetAssignedToCollectionAsync()
		{
			var input = new GetTaskAssignmentsInput();
			input.DocId = DocId;
			input.Url = Url;
			input.MaxResultCount = MaxCount;

			var result = await TaskAssignmentsAppService.GetListAsync(input);

			TaskAssignmentList = (List<TaskAssignmentDto>)result.Items;
			foreach (var item in TaskAssignmentList)
			{
				var assignedUser = await IdentityUserAppService.GetAsync(item.AssignedUserId);
				if (assignedUser != null)
				{
					AssignedUserList.Add(assignedUser);
				}
			}
			SelectedUsers = AssignedUserList;
		}

		private async Task GetSharedCollectionAsync()
		{
			var input = new GetShareWithsInput();
			input.DocId = DocId;
			input.Url = Url;
			input.MaxResultCount = MaxCount;

			var result = await ShareWithsAppService.GetListAsync(input);
			ShareWithList = (List<ShareWithDto>)result.Items;
		}

		private async Task GetSharedUsersAsync()
		{
			SharedUserList = new List<IdentityUserDto>();
			SharedUsers = new List<IdentityUserDto>();

			foreach (var item in ShareWithList)
			{
				var sharedUser = await IdentityUserAppService.GetAsync(item.SharedToUserId);
				if (sharedUser != null)
				{
					SharedUserList.Add(sharedUser);
				}
			}
			SharedUsers = SharedUserList.AsEnumerable();
		}

		private async Task GetNotificationTypeLookupAsync()
		{
			NotificationsTypeList = Enum.GetValues(typeof(NotificationsType))
			.OfType<NotificationsType>()
			.Select(t => new NotificationsTypeList()
			{
				Value = t.ToString(),
				DisplayName = L["Enum:NotificationsType." + t.ToString()],

			}).ToList();
		}

		private async Task OpenTaskAssigmenentModalAsync()
		{

			await EditAssignedToModal.ShowAsync();
		}

		private async Task OpenSharingModalAsync()
		{
			await EditSharingModal.ShowAsync();
		}

		private async Task AddTaskAssigmentAsync(TaskAssignmentData arg)
		{
			if (arg != null)
			{
				var taskAssignmentCreating = arg.TaskAssignment;
				AssignedUserList = arg.SelectedUsers.ToList();
				SelectedUsers = arg.SelectedUsers;
				taskAssignmentCreating.Url = Url;
				taskAssignmentCreating.DocId = DocId;
				notiMessage.DocId = DocId;
				notiMessage.Url = Url;
				notiMessage.FromUserId = (Guid)CurrentUser.Id;

				foreach (var item in SelectedUsers)
				{
					var existingAssignedUser = await TaskAssignmentsAppService.GetListAsync(new GetTaskAssignmentsInput { MaxResultCount = 1, DocId = DocId, AssignedUserId = item.Id, Url = Url });
					if (existingAssignedUser.TotalCount == 0)
					{
						taskAssignmentCreating.AssignedUserId = item.Id;
						notiMessage.ToUserId = item.Id;
						if (string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(TypeTable))
						{
							notiMessage.NotiTitle = CurrentUser.Name + " assigned to you";
						}
						else
						{
							notiMessage.NotiTitle = CurrentUser.Name + " assigned a new " + $"{TypeTable} " + $"in {TableName} to you";
						}
						notiMessage.NotiBody = L["You have been assigned a document."];
						notiMessage.Type = NotificationsType.Assignment;
						await TaskAssignmentsAppService.CreateAsync(taskAssignmentCreating);
						await SendEmailToUser(notiMessage.ToUserId, notiMessage.NotiTitle, notiMessage.NotiBody);
						await NotificationsAppService.CreateAsync(notiMessage);
						await SendMessage();
					}
				}
				foreach (var item in TaskAssignmentList)
				{
					var assignedUser = SelectedUsers.FirstOrDefault(u => u.Id == item.AssignedUserId);
					var result = await NotificationsAppService.GetListAsync(new GetNotificationsInput { MaxResultCount = 1, DocId = DocId, Url = Url, ToUserId = item.AssignedUserId, Type = NotificationsType.Assignment });

					if (assignedUser == null)
					{
						await TaskAssignmentsAppService.DeleteAsync(item.Id);
						if (result.Items.Count > 0)
						{
							var notifiedRecord = (List<NotificationDto>)result.Items;
							notiMessage = ObjectMapper.Map<NotificationDto, NotificationCreateDto>(notifiedRecord.FirstOrDefault());
							await NotificationsAppService.DeleteAsync(notifiedRecord.FirstOrDefault().Id);
							await SendMessage();
						}

					}
				}
			}
			await InvokeAsync(async() =>
			{
				await EditAssignedToModal.CloseAsync();
			});
		}

		private async Task AddShareWithAsync(List<ShareWithDto> arg)
		{
			if (arg != null)
			{
				var notificationCreating = new NotificationUpdateDto();
				notificationCreating.DocId = DocId;
				notificationCreating.Url = Url;
				notificationCreating.FromUserId = (Guid)CurrentUser.Id;

				ShareWithList = arg;
				var shareWithCreating = new ShareWithUpdateDto();
				shareWithCreating.Url = Url;
				shareWithCreating.DocId = DocId;

				foreach (var item in ShareWithList)
				{
					shareWithCreating.SharedToUserId = item.SharedToUserId;
					shareWithCreating.CanRead = item.CanRead;
					shareWithCreating.CanShare = item.CanShare;
					shareWithCreating.CanWrite = item.CanWrite;
					shareWithCreating.CanSubmit = item.CanSubmit;
					shareWithCreating.ConcurrencyStamp = item.ConcurrencyStamp;

					var existingSharedUser = await ShareWithsAppService.GetListAsync(new GetShareWithsInput { MaxResultCount = 1, DocId = DocId, SharedToUserId = item.SharedToUserId, Url = Url, });
					if (existingSharedUser.TotalCount == 0)
					{
						notificationCreating.ToUserId = item.SharedToUserId;
						if (string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(TypeTable))
						{
							notificationCreating.NotiTitle = CurrentUser.Name + " shared with you";
						}
						else
						{
							notificationCreating.NotiTitle = CurrentUser.Name + " shared a " + $"{TypeTable} " + $"in {TableName} with you";
						}
						notificationCreating.NotiBody = L["You have been shared a document."];
						notificationCreating.Type = NotificationsType.Share;
						notiMessage = ObjectMapper.Map<NotificationUpdateDto, NotificationCreateDto>(notificationCreating);
						await ShareWithsAppService.CreateAsync(ObjectMapper.Map<ShareWithUpdateDto, ShareWithCreateDto>(shareWithCreating));
						await SendEmailToUser(notificationCreating.ToUserId, notificationCreating.NotiTitle, notificationCreating.NotiBody);
						await NotificationsAppService.CreateAsync(notiMessage);
						await SendMessage();
					}
				}
			}

			await GetSharedCollectionAsync();
			await GetSharedUsersAsync();


			await InvokeAsync(async() =>
			{
				await EditSharingModal.CloseAsync();
			});
		}

		private async Task DeleteShareWithAsync(NotificationDto arg)
		{
			notiMessage = ObjectMapper.Map<NotificationDto, NotificationCreateDto>(arg);
			await SendMessage();
		}

		int SelectedFilesCount { get; set; }
		protected void SelectedFilesChanged(IEnumerable<UploadFileInfo> files)
		{
			SelectedFilesCount = files.ToList().Count;
			InvokeAsync(StateHasChanged);
		}
		protected string GetUploadUrl(string url)
		{
			return NavigationManager.ToAbsoluteUri(url).AbsoluteUri;
		}



		#region Email

		protected UpdateEmailSettingsViewModel EmailSettings;

		protected SendTestEmailViewModel SendTestEmailInput;

		protected Validations EmailSettingValidation;

		private async Task SendEmailToUser(Guid userId, string subject, string body)
		{
			var sharedUser = await IdentityUserAppService.GetAsync(userId);
			if (sharedUser != null)
			{
				var recipientEmail = sharedUser.Email;
				await SendEmailAsync(recipientEmail, subject, body);
				Console.WriteLine(body);
			}
		}

		private async Task SendEmailAsync(string recipientEmail, string subject, string body)
		{
			var emailSettings = await EmailSettingsAppService.GetAsync();

			// Gửi email sử dụng thông tin cấu hình và tham số truyền vào
			// Code gửi email ở đây
			EmailSettings = ObjectMapper.Map<EmailSettingsDto, UpdateEmailSettingsViewModel>(await EmailSettingsAppService.GetAsync());
			SendTestEmailInput = new SendTestEmailViewModel
			{
				SenderEmailAddress = emailSettings.DefaultFromAddress,
				TargetEmailAddress = recipientEmail,
				Subject = subject,
				Body = body
			};
			await EmailSettingsAppService.SendTestEmailAsync(ObjectMapper.Map<SendTestEmailViewModel, SendTestEmailInput>(SendTestEmailInput));
		}

		#endregion

		#region SignalR
		public async Task GetConnectSignalRAsync()
		{
			var apiURL = Configuration.GetValue<string>("SignalR:Url");
				try
				{
					hubConnection = new HubConnectionBuilder()
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

					hubConnection.On<NotificationCreateDto>("ReceiveMessage",
						async (message) =>
						{
							notiMessages.Add(message);
							Console.WriteLine("Received message from server: " + message);
							await InvokeAsync(StateHasChanged);
						});

					await hubConnection.StartAsync();
					Console.WriteLine("SignalR connected");
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error starting HubConnection: " + ex.Message);
				}
		}
		public async Task SendMessage()
		{
			try
			{
				await hubConnection.SendAsync("SendMessage", notiMessage);
				notiMessage = new NotificationCreateDto();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error sending message: " + ex.Message);
			}
		}
		public void Dispose()
		{
			_ = hubConnection?.DisposeAsync();
		}
		#endregion
	}
}
