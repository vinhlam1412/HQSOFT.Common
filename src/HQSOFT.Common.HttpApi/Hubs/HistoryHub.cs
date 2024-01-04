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
    public class HistoryHub : AbpHub
	{
		[HubRoute("/signalr-hubs/history")]
		public async Task SendHistory()
        {
            await Clients.All.SendAsync("ReceiveHistory");
        }
    }
}
