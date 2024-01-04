using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using HQSOFT.Common.Localization;
using HQSOFT.Common.Web.Menus;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using HQSOFT.Common.Permissions;

namespace HQSOFT.Common.Web;

[DependsOn(
    typeof(CommonApplicationContractsModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule)
    )]
public class CommonWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(CommonResource), typeof(CommonWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CommonWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CommonMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CommonWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<CommonWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CommonWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.
            options.Conventions.AuthorizePage("/Comments/Index", CommonPermissions.Comments.Default);
            options.Conventions.AuthorizePage("/Notifications/Index", CommonPermissions.Notifications.Default);
        });
    }
}