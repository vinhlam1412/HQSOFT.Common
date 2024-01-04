using HQSOFT.Common.Comments;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.ShareWiths;
using HQSOFT.Common.TaskAssignments;

using HQSOFT.Common.TestCommons;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace HQSOFT.Common.EntityFrameworkCore;

[DependsOn(
    typeof(CommonDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
	typeof(AbpSettingManagementEntityFrameworkCoreModule),
	typeof(BlobStoringDatabaseEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule)
)]
[DependsOn(typeof(FileManagementEntityFrameworkCoreModule))]
[DependsOn(typeof(AbpAuditLoggingEntityFrameworkCoreModule))]
public class CommonEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CommonDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<TestCommon, TestCommons.EfCoreTestCommonRepository>();

            options.AddRepository<TaskAssignment, TaskAssignments.EfCoreTaskAssignmentRepository>();

            options.AddRepository<ShareWith, ShareWiths.EfCoreShareWithRepository>();

            options.AddRepository<Notification, Notifications.EfCoreNotificationRepository>();

            options.AddRepository<Comment, Comments.EfCoreCommentRepository>();

        });
    }
}