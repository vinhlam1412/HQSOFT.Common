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
    public abstract class CreateModalModelBase : CommonPageModel
    {
        [BindProperty]
        public NotificationCreateViewModel Notification { get; set; }

        protected INotificationsAppService _notificationsAppService;

        public CreateModalModelBase(INotificationsAppService notificationsAppService)
        {
            _notificationsAppService = notificationsAppService;

            Notification = new();
        }

        public virtual async Task OnGetAsync()
        {
            Notification = new NotificationCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _notificationsAppService.CreateAsync(ObjectMapper.Map<NotificationCreateViewModel, NotificationCreateDto>(Notification));
            return NoContent();
        }
    }

    public class NotificationCreateViewModel : NotificationCreateDto
    {
    }
}