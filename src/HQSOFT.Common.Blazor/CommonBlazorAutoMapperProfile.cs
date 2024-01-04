using HQSOFT.Common.Comments;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.ShareWiths;
using Volo.Abp.AutoMapper;
using HQSOFT.Common.TestCommons;
using AutoMapper;
using HQSOFT.Common.TaskAssignments;
using Volo.FileManagement.Files;

namespace HQSOFT.Common.Blazor;

public class CommonBlazorAutoMapperProfile : Profile
{
    public CommonBlazorAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<TestCommonDto, TestCommonUpdateDto>();
        CreateMap<TestCommonDto, TestCommonCreateDto>();
        CreateMap<TestCommonUpdateDto, TestCommonCreateDto>();

        CreateMap<TaskAssignmentDto, TaskAssignmentUpdateDto>();
        CreateMap<TaskAssignmentDto, TaskAssignmentCreateDto>();
        CreateMap<TaskAssignmentUpdateDto, TaskAssignmentCreateDto>();

        CreateMap<ShareWithDto, ShareWithUpdateDto>();
        CreateMap<ShareWithDto, ShareWithCreateDto>();
        CreateMap<ShareWithUpdateDto, ShareWithCreateDto>();

        CreateMap<NotificationDto, NotificationUpdateDto>();
        CreateMap<NotificationDto, NotificationCreateDto>();
        CreateMap<NotificationUpdateDto, NotificationCreateDto>();

        CreateMap<CreateFileInputWithStream, CreateFileInputWithStream>()
            .MapExtraProperties();

        CreateMap<FileDescriptorDto, FileDescriptorDto>()
            .MapExtraProperties();

        CreateMap<CommentDto, CommentUpdateDto>();
        CreateMap<CommentDto, CommentCreateDto>();
        CreateMap<CommentUpdateDto, CommentCreateDto>();

    }
}