using HQSOFT.Common.Notifications;
using HQSOFT.Common.Comments;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace HQSOFT.Common.MongoDB;

[DependsOn(
    typeof(CommonDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class CommonMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<CommonMongoDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, MongoQuestionRepository>();
             */
            options.AddRepository<Comment, Comments.MongoCommentRepository>();

            options.AddRepository<Notification, Notifications.MongoNotificationRepository>();

        });
    }
}