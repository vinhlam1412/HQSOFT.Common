using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.Identity;
using Volo.FileManagement;
using Volo.Abp.AuditLogging;
using Volo.Abp.SettingManagement;

namespace HQSOFT.Common;

[DependsOn(
    typeof(CommonDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule),
	typeof(AbpSettingManagementApplicationContractsModule),
	typeof(AbpIdentityApplicationContractsModule)
    )]
[DependsOn(typeof(FileManagementApplicationContractsModule))]
[DependsOn(typeof(AbpAuditLoggingApplicationContractsModule))]
public class CommonApplicationContractsModule : AbpModule
{

}
