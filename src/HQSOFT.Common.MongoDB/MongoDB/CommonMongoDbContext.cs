using HQSOFT.Common.Notifications;
using HQSOFT.Common.Comments;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace HQSOFT.Common.MongoDB;

[ConnectionStringName(CommonDbProperties.ConnectionStringName)]
public class CommonMongoDbContext : AbpMongoDbContext, ICommonMongoDbContext
{
    public IMongoCollection<Notification> Notifications => Collection<Notification>();
    public IMongoCollection<Comment> Comments => Collection<Comment>();
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureCommon();

        modelBuilder.Entity<Comment>(b => { b.CollectionName = CommonDbProperties.DbTablePrefix + "Comments"; });

        modelBuilder.Entity<Notification>(b => { b.CollectionName = CommonDbProperties.DbTablePrefix + "Notifications"; });
    }
}