    using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Identity;
using Volo.FileManagement;
using Volo.Abp.AuditLogging;
using Volo.Abp.SettingManagement;
using Volo.Abp.Application.Dtos;

namespace HQSOFT.Common;

[DependsOn(
    typeof(CommonDomainModule),
    typeof(CommonApplicationContractsModule),
    typeof(AbpDddApplicationModule),
	typeof(AbpSettingManagementApplicationModule),
	typeof(AbpAutoMapperModule),
    typeof(AbpIdentityApplicationModule)
    )]
[DependsOn(typeof(FileManagementApplicationModule))]
[DependsOn(typeof(AbpAuditLoggingApplicationModule))]
public class CommonApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {  
        context.Services.AddAutoMapperObjectMapper<CommonApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CommonApplicationModule>(validate: true);
        });
    }
}
