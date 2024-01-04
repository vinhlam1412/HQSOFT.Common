using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using HQSOFT.Common.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.FileManagement;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.AuditLogging;
using Volo.Abp.SettingManagement;

namespace HQSOFT.Common;

[DependsOn(
    typeof(AbpValidationModule),
	typeof(AbpSettingManagementDomainSharedModule),
	typeof(BlobStoringDatabaseDomainSharedModule),
    typeof(AbpDddDomainSharedModule)
)]
[DependsOn(typeof(FileManagementDomainSharedModule))]
[DependsOn(typeof(AbpAuditLoggingDomainSharedModule))]
public class CommonDomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FileManagementModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CommonDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<CommonResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Common");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Common", typeof(CommonResource));
        });
    }
}
