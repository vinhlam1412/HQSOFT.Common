using Localization.Resources.AbpUi;
using HQSOFT.Common.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.SignalR;
using Volo.FileManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using System.Linq;
using Volo.Abp.AuditLogging;
using HQSOFT.eBiz.Main.AuditLogging;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.Extensions.Hosting;
using Abp.MultiTenancy;
using HQSOFT.Common.Hubs;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.SettingManagement;
using Volo.Abp.Application.Dtos;

namespace HQSOFT.Common;

[DependsOn(
    typeof(CommonApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule),
	typeof(AbpSettingManagementHttpApiModule),
	typeof(AbpAspNetCoreSignalRModule)
    )]
[DependsOn(typeof(FileManagementHttpApiModule))]
[DependsOn(typeof(AbpAuditLoggingHttpApiModule))]
public class CommonHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CommonHttpApiModule).Assembly);
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddSignalR();
        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/octet-stream" });
        });
		services.AddRazorComponents()
			.AddInteractiveServerComponents();
	}


	public override void ConfigureServices(ServiceConfigurationContext context)
    { 
        context.Services.AddSignalR();   

		Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CommonResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });

		Configure<AbpAuditingOptions>(options =>
		{
			options.Contributors.Add(new ExtendedAuditLogContributor());
			options.IsEnabledForGetRequests = true;
		});

		Configure<AbpAspNetCoreAuditingOptions>(options =>
		{
			options.IgnoredUrls.Add("/connect/token");
			options.IgnoredUrls.Add("/health-status");
		});
	}
	//public override void OnApplicationInitialization(ApplicationInitializationContext context)
	//{
	//	var app = context.GetApplicationBuilder();

	//	if (!context.GetEnvironment().IsDevelopment())
	//	{
	//		app.UseHsts();
	//	} 
	//	app.UseEndpoints(endpoints =>
	//	{
	//		endpoints.MapHub<MessagingHub>("/range");
	//	}); 
	//}
}
