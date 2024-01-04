using Blazorise;
using HQSOFT.Common.Blazor.Pages.Common;
using HQSOFT.Common.Blazor.Pages.RichTextEdit;
using HQSOFT.Common.Comments;
using HQSOFT.Common.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Blazor;
using Volo.Abp.SettingManagement.Localization;
using static Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup.EmailSettingGroupViewComponent;

namespace HQSOFT.Common.Blazor.Pages.Component
{
	public partial class HQSOFTComment
	{
		[Parameter]
		public Guid DocId { get; set; }
		[Parameter]
		public string Url { get; set; }

		private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
		private string CurrentSorting { get; set; } = string.Empty;
		private int CurrentPage { get; set; } = 1;
		private int TotalCount { get; set; }

		private string contentHTML;
		private string contentText;
		private string contentDelta;

		public string Email = "";
		public string Name = "";
		public string? NotiBody = "";

		protected string? savedContent { get; set; }
		protected bool readOnly { get; set; }

		private HubConnection _hubConnection;
		private Validations CheckNull { get; set; } = new();

		private HQSOFTRichTextEdit richTextEditRef;
		private GetNotificationsInput FilterCommentUser { get; set; }
		private GetNotificationsInput FilterNotificationsList { get; set; }
		private GetCommentsInput FilterCommentsList { get; set; }
		private GetIdentityUsersInput FilterUser { get; set; }
		private NotificationCreateDto _message { get; set; } = new NotificationCreateDto();
		private List<NotificationCreateDto> _messages = new List<NotificationCreateDto>();
		private CommentCreateDto _comment { get; set; } = new CommentCreateDto();
		private List<CommentCreateDto> _comments = new List<CommentCreateDto>();
		private IReadOnlyList<NotificationDto> commentUser { get; set; }
		private IReadOnlyList<IdentityUserDto> UserList { get; set; }
		private IReadOnlyList<NotificationDto> notificationList { get; set; }
		private IReadOnlyList<CommentDto> commentList { get; set; }
		private IEnumerable<string> MentionedUsers { get; set; } = new List<string>();



		/******************************** LẤY DANH SÁCH DỮ LIỆU ********************************/
		protected override async Task OnInitializedAsync()
		{
			try
			{
				Console.WriteLine(NotiBody);
				await GetUserAsync();
				await GetConnectSignalR();
				await GetCommentListAsync();
				await GetNotificationListAsync();
				await GetCommentByUserAsync();
				await base.OnInitializedAsync();
			}
			catch (Exception ex)
			{
				await HandleErrorAsync(ex);
			}
		}

		public HQSOFTComment()
		{
			FilterCommentUser = new GetNotificationsInput
			{
				MaxResultCount = PageSize,
				SkipCount = (CurrentPage - 1) * PageSize,
				Sorting = CurrentSorting
			};
			commentUser = new List<NotificationDto>();

			FilterNotificationsList = new GetNotificationsInput
			{
				MaxResultCount = PageSize,
				SkipCount = (CurrentPage - 1) * PageSize,
				Sorting = CurrentSorting
			};
			notificationList = new List<NotificationDto>();

			FilterCommentsList = new GetCommentsInput
			{
				MaxResultCount = PageSize,
				SkipCount = (CurrentPage - 1) * PageSize,
				Sorting = CurrentSorting
			};
			commentList = new List<CommentDto>();

			FilterUser = new GetIdentityUsersInput
			{
				MaxResultCount = PageSize,
				SkipCount = (CurrentPage - 1) * PageSize,
				Sorting = CurrentSorting
			};
			UserList = new List<IdentityUserDto>();

			ObjectMapperContext = typeof(AbpSettingManagementBlazorModule);
			LocalizationResource = typeof(AbpSettingManagementResource);
		}

		public async Task GetConnectSignalR()
		{
		var apiURL = Configuration.GetValue<string>("SignalR:Url");
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

				_hubConnection.On<NotificationCreateDto>("ReceiveMessage",
					(message) =>
					{
						_messages.Add(message);
						Console.WriteLine("Received message from server: " + message);
						 GetCommentListAsync();
						 GetNotificationListAsync();
						StateHasChanged();
					});
				Console.WriteLine("SignalR connected");
				await _hubConnection.StartAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error starting HubConnection: " + ex.Message);
			}
		}

		private async Task GetUserAsync()
		{
			FilterUser.MaxResultCount = PageSize;
			FilterUser.SkipCount = (CurrentPage - 1) * PageSize;
			FilterUser.Sorting = CurrentSorting;

			var result = await IdentityUserAppService.GetListAsync(FilterUser);
			UserList = result.Items;
			TotalCount = (int)result.TotalCount;
		}

		private async Task GetCommentByUserAsync()
		{
			FilterCommentUser.MaxResultCount = PageSize;
			FilterCommentUser.SkipCount = (CurrentPage - 1) * PageSize;
			FilterCommentUser.ToUserId = CurrentUser.Id;

			var result = await NotificationsAppService.GetListAsync(FilterCommentUser);
			commentUser = result.Items.OrderByDescending(m => m.CreationTime).ToList();
			TotalCount = (int)result.TotalCount;
            await InvokeAsync(StateHasChanged);
        }

		private async Task GetCommentListAsync()
		{
			FilterCommentsList.MaxResultCount = PageSize;
			FilterCommentsList.SkipCount = (CurrentPage - 1) * PageSize;
			FilterCommentsList.Sorting = CurrentSorting;

			var result = await CommentsAppService.GetListAsync(FilterCommentsList);
			commentList = result.Items;
			TotalCount = (int)result.TotalCount;
			await InvokeAsync(StateHasChanged);
		}

		private async Task GetNotificationListAsync()
		{
			FilterNotificationsList.MaxResultCount = PageSize;
			FilterNotificationsList.SkipCount = (CurrentPage - 1) * PageSize;
			FilterNotificationsList.Sorting = CurrentSorting;

			var result = await NotificationsAppService.GetListAsync(FilterNotificationsList);
			notificationList = result.Items;
			TotalCount = (int)result.TotalCount;
            await InvokeAsync(StateHasChanged);
        }

		/******************************** THÊM MỚI VÀO DATABASE ********************************/
		private Guid mentionedUserId = Guid.Empty;
		private string mentionedUserName = "";
		private string mentionedUserEmail = "";

		[Parameter]
		public EventCallback OnCommentAdded { get; set; }

		[Parameter]
		public string TableName { get; set; }

		public async Task CreateCommentAsync()
		{
			try
			{
				await HandleMentionIdsChanged();
				savedContent = await richTextEditRef.GetHTML();
				Console.WriteLine("MentionedUsers.Count(): " + MentionedUsers.Count());
				if (MentionedUsers.Count() > 0)
				{
					foreach (var mentionId in MentionedUsers)
					{
						var mentionedUser = UserList.FirstOrDefault(x => x.Id == (Guid.Parse(mentionId)));
						if (mentionedUser != null)
						{
							// Access the mentioned user's properties here
							mentionedUserId = mentionedUser.Id;
							mentionedUserName = mentionedUser.Name;
							mentionedUserEmail = mentionedUser.Email;
						}
						_message.FromUserId = (Guid)CurrentUser.Id;
						_message.ToUserId = mentionedUserId;
						_message.DocId = DocId;
						_message.Url = Url;

						if (await CheckNull.ValidateAll() == false)
						{
							return;
						}

						else if (_message.ToUserId != Guid.Empty)
						{
							_message.NotiTitle = CurrentUser.Name + " " + "mentioned you in " + $"{TableName}";
							_message.NotiBody = "Comment Body: " + CurrentUser.Name + " " + "mentioned you in " + $"{TableName}";
							_message.Type = NotificationsType.Mention;
							await NotificationsAppService.CreateAsync(_message);
							await SendTestEmailAsync();
						}
					}
					_comment.FromUserId = (Guid)CurrentUser.Id;
					_comment.Content = savedContent;
					_comment.DocId = DocId;
					_comment.Url = Url;
					await CommentsAppService.CreateAsync(_comment);
					await Notify.Success(L["Notification:Sent"]);
					await richTextEditRef.ClearAsync();
					await OnCommentAdded.InvokeAsync();
				}
				else
				{
					_comment.FromUserId = (Guid)CurrentUser.Id;
					_comment.Content = savedContent;
					_comment.DocId = DocId;
					_comment.Url = Url;
					await CommentsAppService.CreateAsync(_comment);
					await Notify.Success(L["Notification:Sent"]);
					await richTextEditRef.ClearAsync();
					await OnCommentAdded.InvokeAsync();
				}
			}
			catch (Exception ex)
			{
				await HandleErrorAsync(ex);
			}
		}

		/********************************* EMAIL *********************************/

		protected UpdateEmailSettingsViewModel EmailSettings;

		protected SendTestEmailViewModel SendTestEmailInput;

		protected Validations EmailSettingValidation;

		protected async Task SendTestEmailAsync()
		{
			try
			{
				foreach (var mentionId in MentionedUsers)
				{
					var mentionedUser = UserList.FirstOrDefault(x => x.Id == (Guid.Parse(mentionId)));
					if (mentionedUser != null)
					{
						mentionedUserId = mentionedUser.Id;
						mentionedUserName = mentionedUser.Name;
						mentionedUserEmail = mentionedUser.Email;
					}
					await GetCommentListAsync();
					await GetNotificationListAsync();
					var latestComment = commentList.OrderByDescending(m => m.CreationTime).FirstOrDefault();
					if (latestComment != null)
					{
						NotiBody = latestComment.Content;
						Console.WriteLine(NotiBody);
					}
					EmailSettings = ObjectMapper.Map<EmailSettingsDto, UpdateEmailSettingsViewModel>(await EmailSettingsAppService.GetAsync());
					var emailSettings = await EmailSettingsAppService.GetAsync();
					SendTestEmailInput = new SendTestEmailViewModel
					{
						SenderEmailAddress = emailSettings.DefaultFromAddress,
						TargetEmailAddress = mentionedUserEmail,
						Subject = "Comment Body: " + CurrentUser.Name + " " + "mentioned you in " + $"{TableName}",
						Body = NotiBody
					};
					await EmailSettingsAppService.SendTestEmailAsync(ObjectMapper.Map<SendTestEmailViewModel, SendTestEmailInput>(SendTestEmailInput));
				}
                await InvokeAsync(StateHasChanged);
            }
			catch (Exception ex)
			{
				await HandleErrorAsync(ex);
			}
		}

		private async Task HandleMentionIdsChanged()
		{
			MentionedUsers = await richTextEditRef.GetMentionedUsers();
		}

		public async Task ClearAsync()
		{
			await richTextEditRef.ClearAsync();
		}

		public async Task OnContentChanged()
		{
			contentHTML = await richTextEditRef.GetHTML();
			contentText = await richTextEditRef.GetText();
			contentDelta = await richTextEditRef.GetContent();
		}

		/********************************* SIGNALR *********************************/

		public async Task SendMessage()
		{
			try
			{
				await _hubConnection.SendAsync("SendMessage", _message);
				_message = new NotificationCreateDto();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error sending message: " + ex.Message);
			}
		}

		public void Dispose()
		{
			_ = _hubConnection?.DisposeAsync();
		}

	}
}
