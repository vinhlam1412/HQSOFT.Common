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
    public abstract class EditModalModelBase : CommonPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public NotificationUpdateViewModel Notification { get; set; }

        protected INotificationsAppService _notificationsAppService;

        public EditModalModelBase(INotificationsAppService notificationsAppService)
        {
            _notificationsAppService = notificationsAppService;

            Notification = new();
        }

        public virtual async Task OnGetAsync()
        {
            var notification = await _notificationsAppService.GetAsync(Id);
            Notification = ObjectMapper.Map<NotificationDto, NotificationUpdateViewModel>(notification);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _notificationsAppService.UpdateAsync(Id, ObjectMapper.Map<NotificationUpdateViewModel, NotificationUpdateDto>(Notification));
            return NoContent();
        }
    }

    public class NotificationUpdateViewModel : NotificationUpdateDto
    {
    }
}