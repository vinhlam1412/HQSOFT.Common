using HQSOFT.Common.AuditLogging;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AuditLogging;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.VirtualFileSystem;
using Volo.FileManagement;

namespace HQSOFT.Common;

[DependsOn(
    typeof(CommonApplicationContractsModule),
	typeof(AbpSettingManagementHttpApiClientModule),
	typeof(AbpHttpClientModule))]
[DependsOn(typeof(FileManagementHttpApiClientModule))]
[DependsOn(typeof(AbpAuditLoggingHttpApiClientModule))]
public class CommonHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add((remoteServiceName, clientBuilder) =>
            {
                clientBuilder.AddHttpMessageHandler<AuditMessageHandler>();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    { 
        context.Services.AddHttpClientProxies(
            typeof(CommonApplicationContractsModule).Assembly,
            CommonRemoteServiceConsts.RemoteServiceName
        );

        context.Services.AddHttpClientProxies(
                            typeof(FileManagementApplicationContractsModule).Assembly,
                            remoteServiceConfigurationName: "FileManagement"
                            );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CommonHttpApiClientModule>();
        });
    }
}
