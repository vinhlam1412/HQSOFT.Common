using Blazorise.RichTextEdit;
using HQSOFT.Common.Blazor.Extraproperties;
using HQSOFT.Common.Blazor.Menus;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.Blazor;
using Volo.Abp.AuditLogging.Blazor.Server;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace HQSOFT.Common.Blazor;

[DependsOn(
    typeof(CommonApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
[DependsOn(typeof(AbpAuditLoggingBlazorServerModule),
	typeof(AbpAuditLoggingBlazorModule))]
public class CommonBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CommonBlazorModule>();
        context.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
		context.Services.AddHttpContextAccessor();
		Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<CommonBlazorAutoMapperProfile>(validate: true);
        });

        //Config Extra Properties
        Configure<AbpHttpClientProxyingOptions>(options =>
        {
            options.QueryStringConverts.Add(typeof(ExtraPropertyDictionary), typeof(ExtraPropertyDictionaryToQueryString));
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(CommonBlazorModule).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CommonMenuContributor());
        });

        context.Services.AddMudServices();
		context.Services.AddMudServices(configure => configure.PopoverOptions.ThrowOnDuplicateProvider = false);
		context.Services.AddDevExpressBlazor();
        context.Services.AddBlazoriseRichTextEdit();
        context.Services.AddBlazoriseRichTextEdit(configure => configure.UseBubbleTheme = true);
        context.Services.AddBlazoriseRichTextEdit(configure => configure.DynamicallyLoadReferences = true);

		//Add custom toolbar to show Notification icon

		context.Services.AddHttpClientProxies(
			typeof(AbpAuditLoggingApplicationContractsModule).Assembly,
			remoteServiceConfigurationName: "AuditLogging");

        //Configure<AbpNavigationOptions>(options =>
        //{
        //    options.MenuContributors.Add(new CommonMenuContributor());
        //});

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new HQSOFTToolbarContributor());
        });

    }
}
