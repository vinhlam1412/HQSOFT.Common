using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.Notifications;

namespace HQSOFT.Common.Notifications
{
    public class NotificationsAppService : NotificationsAppServiceBase, INotificationsAppService
    {
        //<suite-custom-code-autogenerated>
        public NotificationsAppService(INotificationRepository notificationRepository, NotificationManager notificationManager)
            : base(notificationRepository, notificationManager)
        {
        }
        //</suite-custom-code-autogenerated>

        //Write your custom code...
    }
}