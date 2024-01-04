using HQSOFT.Common.Web.Pages.Common.Notifications;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.Web.Pages.Common.Comments;
using Volo.Abp.AutoMapper;
using HQSOFT.Common.Comments;
using AutoMapper;

namespace HQSOFT.Common.Web;

public class CommonWebAutoMapperProfile : Profile
{
    public CommonWebAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<CommentDto, CommentUpdateViewModel>();
        CreateMap<CommentUpdateViewModel, CommentUpdateDto>();
        CreateMap<CommentCreateViewModel, CommentCreateDto>();

        CreateMap<NotificationDto, NotificationUpdateViewModel>();
        CreateMap<NotificationUpdateViewModel, NotificationUpdateDto>();
        CreateMap<NotificationCreateViewModel, NotificationCreateDto>();
    }
}