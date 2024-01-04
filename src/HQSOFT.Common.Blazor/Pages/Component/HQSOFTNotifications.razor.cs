
using HQSOFT.Common.Notifications;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR.Client;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Volo.Abp.Identity;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Http;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Authentication;
using System.Net;

namespace HQSOFT.Common.Blazor.Pages.Component
{
    public partial class HQSOFTNotifications
    {
        [Parameter]
        public Guid PostID { get; set; }
        bool IsOpen { get; set; } = false;

        private HubConnection _hubConnection;

        private int TotalCount { get; set; }
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int PageSize { get; } = 20;


        private List<NotificationCreateDto> _messages = new List<NotificationCreateDto>();
        private NotificationCreateDto _message { get; set; } = new NotificationCreateDto();

        private GetIdentityUsersInput FilterUser { get; set; }
        //private IReadOnlyList<IdentityUserDto> UserList { get; set; }

        private GetNotificationsInput FilterNotificationList { get; set; }
        private IEnumerable<NotificationDto> NotificationList { get; set; }
        private NotificationDto SelectedNotification { get; set; }
        private int UnreadNotiCount { get; set; } = 0;
        private bool ShowNotiDot { get; set; } = false;
        private readonly IConfiguration _configuration;
        private string authServerUrl;


        public HQSOFTNotifications(IConfiguration configuration)
        {
            FilterUser = new GetIdentityUsersInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };

            FilterNotificationList = new GetNotificationsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            NotificationList = new List<NotificationDto>();
            _configuration = configuration;
            authServerUrl = _configuration["AuthServer:Authority"];
        }

        protected override async Task OnInitializedAsync()
        {
            if (CurrentUser.IsAuthenticated)
            {
                await GetConnectSignalR();
                await GetNotificationListAsync();           
            }
            await base.OnInitializedAsync();
        }

        public async Task GetConnectSignalR()
        {
            //var tokenResult = await TokenProvider.RequestAccessToken();
            var apiURL = Configuration.GetValue<string>("SignalR:Url");
            Console.WriteLine("Getting SignalR-Common from appsettings.json: " + apiURL);
            try
            {
                //if (tokenResult.TryGetToken(out var token))
                //{
                    //_hubConnection = new HubConnectionBuilder()
                    //                .WithUrl($"{apiURL}", options =>
                    //                {
                    //                    options.AccessTokenProvider = async () => await Task.FromResult(token.Value);
                    //                }).Build();

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
                            _ = GetNotificationListAsync();
                        });

                    await _hubConnection.StartAsync();
                    Console.WriteLine("SignalR-Common connected");
               // }
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

        private async Task GetNotificationListAsync()
        {
            FilterNotificationList.MaxResultCount = PageSize;
            FilterNotificationList.SkipCount = (CurrentPage - 1) * PageSize;

            var result = await NotificationsAppService.GetListAsync(FilterNotificationList);
            NotificationList = result.Items.Where(m => m.ToUserId == CurrentUser.Id || m.ToUserId == Guid.Empty)
                .OrderByDescending(m => m.CreationTime).ToList();
            TotalCount = (int)result.TotalCount;
            UnreadNotiCount = NotificationList.Count(m => ((m.ToUserId == CurrentUser.Id || m.ToUserId == Guid.Empty) && !m.IsRead));

            if (UnreadNotiCount > 0)
            {
                ShowNotiDot = true;
            }
            await InvokeAsync(StateHasChanged);
        }

        protected void GoToEditPage(NotificationDto context)
        {
            IsOpen = false;
            NavigationManager.NavigateTo($"{context.Url}/{context.DocId}");
        }
    }
}
