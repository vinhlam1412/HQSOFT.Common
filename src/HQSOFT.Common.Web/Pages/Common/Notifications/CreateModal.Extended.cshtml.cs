using HQSOFT.Common.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HQSOFT.Common.Notifications;

namespace HQSOFT.Common.Web.Pages.Common.Notifications
{
    public class CreateModalModel : CreateModalModelBase
    {
        public CreateModalModel(INotificationsAppService notificationsAppService)
            : base(notificationsAppService)
        {
        }
    }
}