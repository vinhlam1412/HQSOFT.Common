using HQSOFT.Common.Comments;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.ShareWiths;
using HQSOFT.Common.TaskAssignments;

using HQSOFT.Common.TestCommons;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace HQSOFT.Common.EntityFrameworkCore;

[ConnectionStringName(CommonDbProperties.ConnectionStringName)]
public interface ICommonDbContext : IEfCoreDbContext
{ 
    DbSet<Comment> Comments { get; set; }
    DbSet<Notification> Notifications { get; set; }
    DbSet<ShareWith> ShareWiths { get; set; }
    DbSet<TaskAssignment> TaskAssignments { get; set; }

    DbSet<TestCommon> TestCommons { get; set; }
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}