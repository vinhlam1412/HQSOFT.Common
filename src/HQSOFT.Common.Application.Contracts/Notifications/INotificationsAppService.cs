using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HQSOFT.Common.Notifications
{
    public partial interface INotificationsAppService : IApplicationService
    {
        Task<PagedResultDto<NotificationDto>> GetListAsync(GetNotificationsInput input);

        Task<NotificationDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<NotificationDto> CreateAsync(NotificationCreateDto input);

        Task<NotificationDto> UpdateAsync(Guid id, NotificationUpdateDto input);
    }
}