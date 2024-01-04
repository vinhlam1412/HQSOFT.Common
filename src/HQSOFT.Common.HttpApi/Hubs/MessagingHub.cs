using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization; 
using HQSOFT.Common.Notifications;
using HQSOFT.Common.Comments;
using Volo.Abp.AuditLogging;
using Microsoft.AspNetCore.Mvc;

namespace HQSOFT.Common.Hubs
{ 
    public class MessagingHub : AbpHub
	{
		[HubRoute("/signalr-hubs/messaging")]
		public async Task SendMessage(NotificationDto notificationMsg)
        {
            // Gửi thông báo đến tài khoản được đề cập bằng SignalR 
            await Clients.All.SendAsync("ReceiveMessage", notificationMsg);
        } 
    }
}
