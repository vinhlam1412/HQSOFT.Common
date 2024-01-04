using HQSOFT.Common.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.Notifications;

namespace HQSOFT.Common.Web.Pages.Common.Notifications
{
    public class EditModalModel : EditModalModelBase
    {
        public EditModalModel(INotificationsAppService notificationsAppService)
            : base(notificationsAppService)
        {
        }
    }
}