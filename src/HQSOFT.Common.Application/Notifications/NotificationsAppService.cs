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

    [Authorize(CommonPermissions.Notifications.Default)]
    public abstract class NotificationsAppServiceBase : ApplicationService
    {

        protected INotificationRepository _notificationRepository;
        protected NotificationManager _notificationManager;

        public NotificationsAppServiceBase(INotificationRepository notificationRepository, NotificationManager notificationManager)
        {

            _notificationRepository = notificationRepository;
            _notificationManager = notificationManager;
        }

        public virtual async Task<PagedResultDto<NotificationDto>> GetListAsync(GetNotificationsInput input)
        {
            var totalCount = await _notificationRepository.GetCountAsync(input.FilterText, input.FromUserId, input.ToUserId, input.NotiTitle, input.NotiBody, input.IsRead, input.DocId, input.Url, input.Type);
            var items = await _notificationRepository.GetListAsync(input.FilterText, input.FromUserId, input.ToUserId, input.NotiTitle, input.NotiBody, input.IsRead, input.DocId, input.Url, input.Type, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<NotificationDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Notification>, List<NotificationDto>>(items)
            };
        }

        public virtual async Task<NotificationDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Notification, NotificationDto>(await _notificationRepository.GetAsync(id));
        }

        [Authorize(CommonPermissions.Notifications.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _notificationRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.Notifications.Create)]
        public virtual async Task<NotificationDto> CreateAsync(NotificationCreateDto input)
        {

            var notification = await _notificationManager.CreateAsync(
            input.FromUserId, input.ToUserId, input.NotiTitle, input.IsRead, input.DocId, input.Url, input.Type, input.NotiBody
            );

            return ObjectMapper.Map<Notification, NotificationDto>(notification);
        }

        [Authorize(CommonPermissions.Notifications.Edit)]
        public virtual async Task<NotificationDto> UpdateAsync(Guid id, NotificationUpdateDto input)
        {

            var notification = await _notificationManager.UpdateAsync(
            id,
            input.FromUserId, input.ToUserId, input.NotiTitle, input.IsRead, input.DocId, input.Url, input.Type, input.NotiBody, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Notification, NotificationDto>(notification);
        }
    }
}