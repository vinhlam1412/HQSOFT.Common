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
    public abstract class IndexModelBase : AbpPageModel
    {
        public string? FromUserIdFilter { get; set; }
        public string? ToUserIdFilter { get; set; }
        public string? NotiTitleFilter { get; set; }
        public string? NotiBodyFilter { get; set; }
        [SelectItems(nameof(IsReadBoolFilterItems))]
        public string IsReadFilter { get; set; }

        public List<SelectListItem> IsReadBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        public string? DocIdFilter { get; set; }
        public string? UrlFilter { get; set; }
        public NotificationsType? TypeFilter { get; set; }

        protected INotificationsAppService _notificationsAppService;

        public IndexModelBase(INotificationsAppService notificationsAppService)
        {
            _notificationsAppService = notificationsAppService;
        }

        public virtual async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}