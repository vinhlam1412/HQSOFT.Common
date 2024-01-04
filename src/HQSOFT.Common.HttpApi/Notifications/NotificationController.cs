using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.Notifications;

namespace HQSOFT.Common.Notifications
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("Notification")]
    [Route("api/common/notifications")]
    public class NotificationController : AbpController, INotificationsAppService
    {
        private readonly INotificationsAppService _notificationsAppService;

        public NotificationController(INotificationsAppService notificationsAppService)
        {
            _notificationsAppService = notificationsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<NotificationDto>> GetListAsync(GetNotificationsInput input)
        {
            return _notificationsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<NotificationDto> GetAsync(Guid id)
        {
            return _notificationsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<NotificationDto> CreateAsync(NotificationCreateDto input)
        {
            return _notificationsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<NotificationDto> UpdateAsync(Guid id, NotificationUpdateDto input)
        {
            return _notificationsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _notificationsAppService.DeleteAsync(id);
        }
    }
}