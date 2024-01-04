using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;

namespace HQSOFT.Common.EntityFrameworkCore;

public class CommonHttpApiHostMigrationsDbContext : AbpDbContext<CommonHttpApiHostMigrationsDbContext>
{
    public CommonHttpApiHostMigrationsDbContext(DbContextOptions<CommonHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureFileManagement();
        modelBuilder.ConfigureCommon();
        modelBuilder.ConfigureAuditLogging();
        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureSettingManagement();
    }
}
