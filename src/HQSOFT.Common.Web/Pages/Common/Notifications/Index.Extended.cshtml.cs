using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Web.Pages.Common.Notifications
{
    public class IndexModel : IndexModelBase
    {
        public IndexModel(INotificationsAppService notificationsAppService)
            : base(notificationsAppService)
        {
        }
    }
}