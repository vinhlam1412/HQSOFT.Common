using HQSOFT.Common.Comments;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.ShareWiths;
using HQSOFT.Common.TaskAssignments;

using HQSOFT.Common.TestCommons;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;

namespace HQSOFT.Common.EntityFrameworkCore;

[ConnectionStringName(CommonDbProperties.ConnectionStringName)]
public class CommonDbContext : AbpDbContext<CommonDbContext>, ICommonDbContext
{
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<ShareWith> ShareWiths { get; set; } = null!;
    public DbSet<TaskAssignment> TaskAssignments { get; set; } = null!;
    public DbSet<TestCommon> TestCommons { get; set; } = null!;
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public CommonDbContext(DbContextOptions<CommonDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCommon();
        builder.ConfigureFileManagement();
    }
}