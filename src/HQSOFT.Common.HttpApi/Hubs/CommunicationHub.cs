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
	public class CommunicationHub : AbpHub
	{
		[HubRoute("/signalr-hubs/communication")]
		public async Task SendCommunication()
        {
            await Clients.All.SendAsync("ReceiveCommunication");
        }
         
    }
}
