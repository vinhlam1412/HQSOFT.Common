using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.FileManagement;

namespace HQSOFT.Common;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
	typeof(AbpSettingManagementDomainModule),
	typeof(CommonDomainSharedModule)
)]
[DependsOn(typeof(FileManagementDomainModule))]
[DependsOn(typeof(AbpAuditLoggingDomainModule))]
public class CommonDomainModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		Configure<AbpAuditingOptions>(options =>
		{
			options.EntityHistorySelectors.AddAllEntities();
			options.DisableLogActionInfo = true;
			options.IsEnabledForIntegrationServices = true;
		});
	}
}
