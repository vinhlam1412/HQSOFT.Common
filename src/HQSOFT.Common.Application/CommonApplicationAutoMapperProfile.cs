using HQSOFT.Common.Comments;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.ShareWiths;
using HQSOFT.Common.TaskAssignments;

using System;
using HQSOFT.Common.Shared;
using Volo.Abp.AutoMapper;
using HQSOFT.Common.TestCommons;
using AutoMapper;

namespace HQSOFT.Common;

public class CommonApplicationAutoMapperProfile : Profile
{
    public CommonApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<TestCommon, TestCommonDto>()
            .Ignore(x => x.IsChanged);
        CreateMap<TestCommon, TestCommonExcelDto>();

        CreateMap<TaskAssignment, TaskAssignmentDto>()
            .Ignore(x => x.IsChanged);

        CreateMap<TaskAssignment, TaskAssignmentExcelDto>();

        CreateMap<ShareWith, ShareWithDto>()
            .Ignore(x => x.IsChanged);

        CreateMap<Notification, NotificationDto>()
            .Ignore(x => x.IsChanged);

        CreateMap<Comment, CommentDto>().Ignore(x => x.IsChanged);
        CreateMap<Comment, CommentExcelDto>();

        CreateMap<Notification, NotificationDto>().Ignore(x => x.IsChanged);
    }
}