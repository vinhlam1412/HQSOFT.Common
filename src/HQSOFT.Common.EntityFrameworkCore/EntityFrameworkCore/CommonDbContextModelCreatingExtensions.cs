using HQSOFT.Common.Comments;
using HQSOFT.Common.Notifications;
using HQSOFT.Common.ShareWiths;
using HQSOFT.Common.TaskAssignments;

using Volo.Abp.EntityFrameworkCore.Modeling;
using HQSOFT.Common.TestCommons;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace HQSOFT.Common.EntityFrameworkCore;

public static class CommonDbContextModelCreatingExtensions
{
    public static void ConfigureCommon(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(CommonDbProperties.DbTablePrefix + "Questions", CommonDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
        if (builder.IsHostDatabase())
        {
            builder.Entity<TestCommon>(b =>
            {
                b.ToTable(CommonDbProperties.DbTablePrefix + "TestCommons", CommonDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Code).HasColumnName(nameof(TestCommon.Code));
                b.Property(x => x.Name).HasColumnName(nameof(TestCommon.Name));
                b.Property(x => x.Idx).HasColumnName(nameof(TestCommon.Idx));
            });

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<TaskAssignment>(b =>
            {
                b.ToTable(CommonDbProperties.DbTablePrefix + "TaskAssignments", CommonDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => new { x.DocId, x.AssignedUserId, x.Url }).IsUnique();
                b.Property(x => x.DocId).HasColumnName(nameof(TaskAssignment.DocId));
                b.Property(x => x.Url).HasColumnName(nameof(TaskAssignment.Url)).IsRequired();
                b.Property(x => x.DueDate).HasColumnName(nameof(TaskAssignment.DueDate));
                b.Property(x => x.Priority).HasColumnName(nameof(TaskAssignment.Priority));
                b.Property(x => x.Comment).HasColumnName(nameof(TaskAssignment.Comment));
                b.Property(x => x.AssignedUserId).HasColumnName(nameof(TaskAssignment.AssignedUserId));
            });

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<ShareWith>(b =>
            {
                b.ToTable(CommonDbProperties.DbTablePrefix + "ShareWiths", CommonDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => new { x.DocId, x.SharedToUserId, x.Url }).IsUnique();
                b.Property(x => x.DocId).HasColumnName(nameof(ShareWith.DocId));
                b.Property(x => x.CanRead).HasColumnName(nameof(ShareWith.CanRead));
                b.Property(x => x.CanWrite).HasColumnName(nameof(ShareWith.CanWrite));
                b.Property(x => x.CanSubmit).HasColumnName(nameof(ShareWith.CanSubmit));
                b.Property(x => x.CanShare).HasColumnName(nameof(ShareWith.CanShare));
                b.Property(x => x.Url).HasColumnName(nameof(ShareWith.Url));
                b.Property(x => x.SharedToUserId).HasColumnName(nameof(ShareWith.SharedToUserId));
            });

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Comment>(b =>
            {
                b.ToTable(CommonDbProperties.DbTablePrefix + "Comments", CommonDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FromUserId).HasColumnName(nameof(Comment.FromUserId));
                b.Property(x => x.Content).HasColumnName(nameof(Comment.Content));
                b.Property(x => x.DocId).HasColumnName(nameof(Comment.DocId));
                b.Property(x => x.Url).HasColumnName(nameof(Comment.Url));
            });

            if (builder.IsHostDatabase())
            {

            }
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Notification>(b =>
{
    b.ToTable(CommonDbProperties.DbTablePrefix + "Notifications", CommonDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.FromUserId).HasColumnName(nameof(Notification.FromUserId)).IsRequired();
    b.Property(x => x.ToUserId).HasColumnName(nameof(Notification.ToUserId));
    b.Property(x => x.NotiTitle).HasColumnName(nameof(Notification.NotiTitle)).IsRequired();
    b.Property(x => x.NotiBody).HasColumnName(nameof(Notification.NotiBody));
    b.Property(x => x.IsRead).HasColumnName(nameof(Notification.IsRead)).IsRequired();
    b.Property(x => x.DocId).HasColumnName(nameof(Notification.DocId)).IsRequired();
    b.Property(x => x.Url).HasColumnName(nameof(Notification.Url)).IsRequired();
    b.Property(x => x.Type).HasColumnName(nameof(Notification.Type));
});

        }
    }
}