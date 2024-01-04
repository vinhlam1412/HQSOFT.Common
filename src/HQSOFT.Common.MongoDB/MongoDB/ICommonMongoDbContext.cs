using HQSOFT.Common.Notifications;
using HQSOFT.Common.Comments;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace HQSOFT.Common.MongoDB;

[ConnectionStringName(CommonDbProperties.ConnectionStringName)]
public interface ICommonMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<Notification> Notifications { get; }
    IMongoCollection<Comment> Comments { get; }
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}