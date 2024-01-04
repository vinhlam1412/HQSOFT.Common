
using Abp.Collections.Extensions;
using Abp.EntityHistory;
using HQSOFT.Common.Blazor.Pages.RichTextEdit;
using HQSOFT.Common.Comments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Identity;

namespace HQSOFT.Common.Blazor.Pages.Component
{
	public partial class HQSOFTFormActivity
	{
		public const string UrlName = "ScreenUrl";
		public const string UserIdName = "UserId";

		private string authServerUrl;
		private readonly IConfiguration _configuration;
		private readonly IAuditingManager _auditingManager;

		[Parameter]
		public Guid DocId { get; set; }

		[Parameter]
		public string Url { get; set; }

		[Parameter]
		public bool IsActive { get; set; }


		private HQSOFTRichTextEdit richTextEditRef;
		private string contentHTML;
		private string contentText;
		private string contentDelta;

		private Guid? userId = Guid.Empty;

		Guid editingCommentId = Guid.Empty; // Biến để lưu trữ Id của comment đang được chỉnh sửa 

		private string desiredUrl = "";
		private bool IsEditing { get; set; } = false;

		private List<string> urlEntityList = new List<string>();

		private List<string> docEntityList = new List<string>();

		private List<string> userIdList = new List<string>();
		private List<string> urlDocEntityList = new List<string>();
		private List<string> MentionedUserIds { get; set; } = new List<string>();
		private int TotalCount { get; set; }
		private int CurrentPage { get; set; } = 1;
		private string CurrentSorting { get; set; } = string.Empty;
		private GetCommentsInput FilterCommentList { get; set; }
		private IReadOnlyList<CommentDto> commentList { get; set; }
		private List<CommentDto> commentsList { get; set; } = new List<CommentDto>();

		private List<AuditLogDto> auditLogsList { get; set; } = new List<AuditLogDto>();
		private List<EntityChangeDto> entityChangesList { get; set; } = new List<EntityChangeDto>();
		private IReadOnlyList<EntityChangeDto> entityChangeList { get; set; } = new List<EntityChangeDto>();
		private GetIdentityUsersInput FilterUser { get; set; }
		private IReadOnlyList<IdentityUserDto> UserList { get; set; }

		private HubConnection _hubConnection;

		protected override async Task OnInitializedAsync()
		{
			await GetUserAsync();
			await GetCommentListAsync();
			await InvokeAsync(StateHasChanged);
			await base.OnInitializedAsync();
		}
		public HQSOFTFormActivity(IConfiguration configuration, IAuditingManager auditingManager)
		{
			_configuration = configuration;
			_auditingManager = auditingManager;
			authServerUrl = _configuration["AuthServer:Authority"];

			FilterUser = new GetIdentityUsersInput
			{
				Sorting = CurrentSorting
			};
			UserList = new List<IdentityUserDto>();

			FilterCommentList = new GetCommentsInput
			{
				Sorting = CurrentSorting
			};
			commentList = new List<CommentDto>();
		}

		private async Task GetUserAsync()
		{
			FilterUser.Sorting = CurrentSorting;

			var result = await IdentityUserAppService.GetListAsync(FilterUser);
			UserList = result.Items;
			TotalCount = (int)result.TotalCount;
			await InvokeAsync(StateHasChanged);
		}

		public async Task GetCommentListAsync()
		{
			var result = await CommentsAppService.GetListAsync(FilterCommentList);
			commentsList = result.Items.OrderByDescending(m => m.CreationTime).ToList();
			TotalCount = (int)result.TotalCount;
			await InvokeAsync(StateHasChanged);
		}

		async Task UpdateCommentContent(Guid commentId, string newContent)
		{
			try
			{
				CommentDto comment = await CommentsAppService.GetAsync(commentId); // Lấy thông tin comment hiện tại từ cơ sở dữ liệu
				CommentUpdateDto updateDto = new CommentUpdateDto
				{
					Id = commentId,
					Content = newContent,
					DocId = comment.DocId,
					Url = comment.Url,
					FromUserId = comment.FromUserId,
					ConcurrencyStamp = comment.ConcurrencyStamp // Truyền giá trị của ConcurrencyStamp từ comment hiện tại
				};

				await CommentsAppService.UpdateAsync(commentId, updateDto);
				await Notify.Success(L["Notification:Edit"]);
				await GetCommentListAsync();
			}
			catch (Exception ex)
			{
				await HandleErrorAsync(ex);
			}
		}

		[Parameter]
		public List<Guid> DocIds { get; set; } = new List<Guid>();

		public async Task GetEntityChangeAsync()
		{
			var entityChanges = await AuditLogsAppService.GetEntityChangesAsync(new GetEntityChangesDto { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount });
			entityChangeList = entityChanges.Items;

			Console.WriteLine("COUNT: " + entityChangeList.Count());
            await InvokeAsync(StateHasChanged);
        }

		public async Task GetHistoryListAsync()
		{
			var tempEntityChangesList = new List<EntityChangeDto>();
			var tempUrlList = new List<string>();

			string UrlDocId = Url + "/" + DocId;

			entityChangesList.Clear();

			urlDocEntityList.Clear();
			urlEntityList.Clear();
			docEntityList.Clear();
			userIdList.Clear();

			var checkExtra = entityChangeList.Select(c => c.ExtraProperties);

			foreach (var entityChange in entityChangeList.Where(x => x.EntityTypeFullName != "Volo.Abp.OpenIddict.Tokens.OpenIddictToken" && x.EntityTypeFullName != "Volo.Abp.Identity.IdentitySecurityLog" &&
				((x.ExtraProperties != null && x.ExtraProperties.Count() > 0 && GetDesiredUrl(x.ExtraProperties[UrlName]?.ToString()) == Url && x.EntityId == DocId.ToString()) ||
				(x.ExtraProperties != null && x.ExtraProperties.Count() > 0 && GetDesiredDocId(x.ExtraProperties[UrlName]?.ToString()) == DocId.ToString()) &&
				(x.ChangeType == EntityChangeType.Created || x.ChangeType == EntityChangeType.Updated))
			))
			{
				var extraProperties = entityChange.ExtraProperties;
				var docUrl = extraProperties[UrlName];
				string desiredUrl = GetDesiredUrlDocId(docUrl.ToString());

				var extraProperties1 = entityChange.ExtraProperties;
				var docUrl1 = extraProperties[UserIdName];
				string desiredUrl1 = docUrl1.ToString();

				var extraProperties2 = entityChange.ExtraProperties;
				var docUrl2 = extraProperties[UrlName];
				string desiredUrl2 = GetDesiredDocId(docUrl.ToString());

				var extraProperties3 = entityChange.ExtraProperties;
				var docUrl3 = extraProperties[UrlName];
				string desiredUrl3 = GetDesiredUrl(docUrl.ToString());

				entityChangesList.Add(entityChange);

				urlEntityList.Add(desiredUrl3);
				docEntityList.Add(desiredUrl2);
				urlDocEntityList.Add(desiredUrl2);
				userIdList.Add(desiredUrl1);
			}
            await InvokeAsync(StateHasChanged);
        }

		public async Task SetIsNotEditing(Guid commentId)
		{
			if (editingCommentId == commentId)
			{
				string newContent = await richTextEditRef.GetHTML(); // Lấy nội dung đã chỉnh sửa từ trình soạn thảo
				await UpdateCommentContent(editingCommentId, newContent); // Cập nhật nội dung vào cơ sở dữ liệu
				IsEditing = false;
                await InvokeAsync(StateHasChanged);  // Cập nhật giao diện người dùng
			}
		}

		public async Task SetIsEditing(Guid commentId)
		{
			editingCommentId = commentId;
			IsEditing = true;
            await InvokeAsync(StateHasChanged); // Cập nhật giao diện người dùng
		}

		public async Task CancelEditing(Guid commentId)
		{
			if (editingCommentId == commentId)
			{
				IsEditing = false;
                await InvokeAsync(StateHasChanged);
            }
		}

		public async Task HandleCommentAdded()
		{
			await GetCommentListAsync();
            await InvokeAsync(StateHasChanged);
        }

		public string AddSpaceToCamelCase(string input)
		{
			string output = Regex.Replace(input, "(?<!^)([A-Z])", " $1");
			return output;
		}

		public string GetLastSegmentAfterDot(string data)
		{
			string[] segments = data.Split('.');
			string lastSegment = segments[^1];
			string formattedSegment = AddSpaceToCamelCase(lastSegment);
			return formattedSegment;
		}


		#region Add Spaces
		public string AddSpaces(string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1").Trim();
		}
		#endregion


		#region Truncate Text 
		public static string TruncateText(string text, int maxLength) // Cắt chuỗi
		{
			if (text.Length <= maxLength)
				return text;

			return text.Substring(0, maxLength) + "...";
		}
		#endregion

		#region Get Uri
		public string GetDesiredUrl(string inputUrl)
		{
			if (inputUrl != null)
			{
				string trimmedData = inputUrl.Trim('[', ']');
				string cleanedData = trimmedData.Replace("\\", "").Replace("\"", ""); // Loại bỏ ký tự "\", "\" và ký tự nháy kép "\"" 
				string url = cleanedData;// Lấy URL từ chuỗi đã được làm sạch 
				string[] parts = url.Split('/'); // Tách chuỗi URL thành các phần tử 
				if (parts.Length >= 4) // Kiểm tra đủ phần tử để tạo URL
				{
					// Lấy phần "/AccountReceivable/CustomerClasses/" từ phần tử thứ 3 và thứ 4
					desiredUrl = "/" + parts[3] + "/" + parts[4] + "/";
					// Cắt bỏ ký tự "/" cuối cùng
					desiredUrl = desiredUrl.TrimEnd('/');
				}
				else
				{
					desiredUrl = "/";
				}
			}
			return desiredUrl;
		}
		public string GetDesiredUrlDocId(string inputUrl)
		{
			if (inputUrl != null)
			{
				string trimmedData = inputUrl.Trim('[', ']');
				string cleanedData = trimmedData.Replace("\\", "").Replace("\"", ""); // Loại bỏ ký tự "\", "\" và ký tự nháy kép "\"" 
				string url = cleanedData;// Lấy URL từ chuỗi đã được làm sạch 
				string[] parts = url.Split('/'); // Tách chuỗi URL thành các phần tử 
				if (parts.Length >= 5) // Kiểm tra đủ phần tử để tạo URL
				{
					// Lấy phần "/AccountReceivable/CustomerClasses/Id" từ phần tử thứ 3 và thứ 4
					desiredUrl = "/" + parts[3] + "/" + parts[4] + "/" + parts[5] + "/";
					// Cắt bỏ ký tự "/" cuối cùng
					desiredUrl = desiredUrl.TrimEnd('/');
				}
				else
				{
					desiredUrl = "/" + Guid.Empty.ToString();
				}
			}
			return desiredUrl;
			// Thực hiện các thao tác trên inputUrl ở đây
		}

		public string GetDesiredDocId(string inputUrl)
		{
			if (inputUrl != null)
			{
				string trimmedData = inputUrl.Trim('[', ']');
				string cleanedData = trimmedData.Replace("\\", "").Replace("\"", ""); // Loại bỏ ký tự "\", "\" và ký tự nháy kép "\""
				string url = cleanedData; // Lấy URL từ chuỗi đã được làm sạch
				string[] parts = url.Split('/'); // Tách chuỗi URL thành các phần tử

				// Kiểm tra số lượng phần tử và kiểu Guid của phần tử cuối cùng
				if (parts.Length > 0 && Guid.TryParse(parts[^1], out Guid guidValue))
				{
					return guidValue.ToString();
				}
				else
				{
					return Guid.Empty.ToString();
				}
			}

			return Guid.Empty.ToString();
		}

		public string GetDesiredUserId(string inputUrl)
		{
			if (inputUrl != null)
			{
				string[] parts = inputUrl.Split(':');
				if (parts.Length >= 2)
				{
					string id = parts[1].Trim().Trim('"');
					return id;
				}
				else
				{
					desiredUrl = Guid.Empty.ToString();
				}
			}
			return string.Empty; // or handle the case when the ID cannot be extracted
		}

		#endregion

		#region Get Time 
		public string GetTimeAgo(DateTime creationTime)
		{
			TimeSpan timeDifference = DateTime.Now - creationTime;

			if (timeDifference.TotalSeconds < 60)
			{
				return $"{(int)timeDifference.TotalSeconds} seconds ago";
			}
			else if (timeDifference.TotalMinutes < 60)
			{
				return $"{(int)timeDifference.TotalMinutes} minutes ago";
			}
			else if (timeDifference.TotalHours < 24)
			{
				return $"{(int)timeDifference.TotalHours} hours ago";
			}
			else if (timeDifference.TotalDays < 7)
			{
				return $"{(int)timeDifference.TotalDays} days ago";
			}
			else if (timeDifference.TotalDays < 30)
			{
				int weeks = (int)(timeDifference.TotalDays / 7);
				return $"{weeks} weeks ago";
			}
			else if (timeDifference.TotalDays < 30)
			{
				int months = (int)(timeDifference.TotalDays / 30);
				return $"{months} months ago";
			}
			else
			{
				int years = (int)(timeDifference.TotalDays / 365);
				return $"{years} years ago";
			}
		}
		#endregion


		#region SignalR
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

                _hubConnection.On("ReceiveHistory",
					() =>
					{
						CallDataLoad();
						StateHasChanged();
					});

					await _hubConnection.StartAsync();
					await GetEntityChangeAsync();
					await GetHistoryListAsync();
				
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error starting HubConnection: " + ex.Message);
			}
		}

		private void CallDataLoad()
		{
			Task.Run(async () =>
			{
				await GetEntityChangeAsync();
				await GetHistoryListAsync();
			});
		}

		public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

		public void Dispose()
		{
			_ = _hubConnection.DisposeAsync();
		}

		#endregion
	}
}
