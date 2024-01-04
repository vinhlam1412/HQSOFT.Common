using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HQSOFT.Common.EntityFrameworkCore;

public class AuthServerHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<AuthServerHostMigrationsDbContext>
{
    public AuthServerHostMigrationsDbContext CreateDbContext(string[] args)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<AuthServerHostMigrationsDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));

        return new AuthServerHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
