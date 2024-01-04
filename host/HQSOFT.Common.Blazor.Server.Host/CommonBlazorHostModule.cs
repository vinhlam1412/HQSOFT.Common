using System;
using System.IO;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using HQSOFT.Common.Blazor.Server.Host.Menus;
using HQSOFT.Common.EntityFrameworkCore;
using HQSOFT.Common.Localization;
using HQSOFT.Common;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Pro.Admin.Blazor.Server;
using Volo.Abp.Account.Public.Web.ExternalProviders;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Components.Server.LeptonTheme;
using Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.Blazor.Server;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Pro.Blazor.Server;
using Volo.Abp.LeptonTheme.Management;
using Volo.Abp.LeptonTheme.Management.Blazor.Server;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Blazor.Server;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Host;
using Volo.Saas.Host.Blazor.Server;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.UI.Navigation.Urls;
using HQSOFT.Common.MultiTenancy;
using Volo.Abp.Account.Pro.Public.Blazor.Server;
using Microsoft.AspNetCore.Authentication;
using Volo.Abp.Security.Claims;
using MudBlazor.Services;
using Volo.Abp.Bundling;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components;
using HQSOFT.Common.Blazor.Menus;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using CommonMenuContributor = HQSOFT.Common.Blazor.Server.Host.Menus.CommonMenuContributor;

namespace HQSOFT.Common.Blazor.Server.Host;

[DependsOn(
    typeof(CommonApplicationModule),
    typeof(CommonEntityFrameworkCoreModule),
    typeof(CommonHttpApiModule),
    typeof(AbpAspNetCoreMvcUiLeptonThemeModule),
    typeof(AbpAutofacModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreComponentsServerLeptonThemeModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementBlazorServerModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityProEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpFeatureManagementBlazorServerModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpAccountPublicWebOpenIddictModule),
    typeof(AbpAccountAdminBlazorServerModule),
    typeof(AbpAccountPublicBlazorServerModule),
    typeof(AbpAccountAdminHttpApiModule),
    typeof(AbpAccountPublicApplicationModule),
    typeof(AbpAccountPublicHttpApiModule),
    typeof(AbpAccountAdminApplicationModule),
    typeof(AbpIdentityProBlazorServerModule),
    typeof(LeptonThemeManagementBlazorServerModule),
    typeof(LeptonThemeManagementApplicationModule),
    typeof(LeptonThemeManagementDomainModule),
    typeof(SaasHostBlazorServerModule),
    typeof(SaasHostApplicationModule),
    typeof(SaasEntityFrameworkCoreModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(CommonBlazorServerModule)
)]
public class CommonBlazorHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(CommonResource),
                typeof(CommonDomainModule).Assembly,
                typeof(CommonDomainSharedModule).Assembly,
                typeof(CommonApplicationModule).Assembly,
                typeof(CommonApplicationContractsModule).Assembly,
                typeof(CommonBlazorHostModule).Assembly
            );
        });

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Common");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureRenderModes(context);
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        //      context.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        //  {
        //      options.ResponseType = OpenIdConnectResponseType.Code;
        //      options.SaveTokens = true;
        //  });
        //context.Services.AddScoped<TokenProvider>();
        //context.Services.AddHttpClient();

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new HQSOFTToolbarContributor());
        });

        context.Services.AddDevExpressBlazor();
        context.Services.AddMudServices();
        context.Services.AddMudServices(configure => configure.PopoverOptions.ThrowOnDuplicateProvider = false);

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
               BlazorLeptonThemeBundles.Styles.Global,
               bundle =>
               {
                   bundle.AddFiles("/global-styles.css");
                   bundle.AddFiles("/blazor-global-styles.css");
                   bundle.AddFiles("https://use.fontawesome.com/releases/v5.15.4/css/all.css");
                   bundle.AddFiles("/custom.css");
                   bundle.AddFiles("/bootstrap-external.bs4.min.css");
                   bundle.AddFiles("/bootstrap-external.bs5.min.css");
                   bundle.AddFiles("https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css");
                   bundle.AddFiles("/_content/Blazorise/blazorise.css");
                   bundle.AddFiles("/_content/Blazorise.Bootstrap5/blazorise.bootstrap5.css");
                   bundle.AddFiles("/_content/Blazorise.Snackbar/blazorise.snackbar.css");
                   bundle.AddFiles("/_content/DevExpress.Blazor.Themes/bootstrap-external.bs5.min.css");
                   bundle.AddFiles("/_content/DevExpress.Blazor.Themes/bootstrap-external.bs4.min.css");
                   bundle.AddFiles("https://cdn.quilljs.com/1.3.6/quill.snow.css");
                   bundle.AddFiles("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap");
                   bundle.AddFiles("/css/quill/quill.snow.css");
                   bundle.AddFiles("/css/quill/quill.mention.min.css");
                   //bundle.AddFiles("/css/quill/quill.bubble.css");
                   //bundle.AddFiles("/css/quill/quill.core.css");
               }
           );

            options.ScriptBundles.Configure(
                BlazorLeptonThemeBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddFiles("https://cdn.jsdelivr.net/npm/jquery@3.5.1/dist/jquery.slim.min.js");
                    bundle.AddFiles("https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js");
                    bundle.AddFiles("https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.min.js");
                    bundle.AddFiles("https://cdn.quilljs.com/1.3.6/quill.js");
                    bundle.AddFiles("/js/quill/image-resize.min.js");
                    bundle.AddFiles("/js/quill/quill.mention.min.js");
                    bundle.AddFiles("/js/webcam.js");
                    bundle.AddFiles("/js/Configuration.js");
                    bundle.AddFiles("/js/quill/HQSoftRichTextEdit.js");
                    bundle.AddFiles("/_content/MudBlazor/MudBlazor.min.js");
                    //bundle.AddFiles("/js/blazor.webview.net8-p7fix.js");
                    //bundle.AddFiles("/js/quill/clearTextQuill.js");
                    //bundle.AddFiles("/js/quill/ImageResize.js");
                    //bundle.AddFiles("/js/quill/quill.js");
                    //bundle.AddFiles("/js/quill/quillMention.js");
                }
            );
        });

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<CommonDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HQSOFT.Common.Domain.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CommonDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HQSOFT.Common.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CommonApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HQSOFT.Common.Application.Contracts", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CommonApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HQSOFT.Common.Application", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<CommonBlazorHostModule>(hostingEnvironment.ContentRootPath);
            });
        }

        context.Services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Common API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );


        context.Services.AddAuthentication()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClaimActions.MapJsonKey(AbpClaimTypes.Picture, "picture");
            })
            .WithDynamicOptions<GoogleOptions, GoogleHandler>(
                GoogleDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ClientId);
                    options.WithProperty(x => x.ClientSecret, isSecret: true);
                }
            )
            .AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options =>
            {
                //Personal Microsoft accounts as an example.
                options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";

                options.ClaimActions.MapCustomJson("picture", _ => "https://graph.microsoft.com/v1.0/me/photo/$value");
                options.SaveTokens = true;
            })
            .WithDynamicOptions<MicrosoftAccountOptions, MicrosoftAccountHandler>(
                MicrosoftAccountDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ClientId);
                    options.WithProperty(x => x.ClientSecret, isSecret: true);
                }
            )
            .AddTwitter(TwitterDefaults.AuthenticationScheme, options =>
            {
                options.ClaimActions.MapJsonKey(AbpClaimTypes.Picture, "profile_image_url_https");
                options.RetrieveUserDetails = true;
            })
            .WithDynamicOptions<TwitterOptions, TwitterHandler>(
                TwitterDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ConsumerKey);
                    options.WithProperty(x => x.ConsumerSecret, isSecret: true);
                }
            );

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية", "ae"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish", "fi"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français", "fr"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский", "ru"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak", "sk"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español"));
        });

        context.Services
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CommonMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(CommonBlazorHostModule).Assembly;
        });

            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
            });

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
    }

    public void ConfigureRenderModes(ServiceConfigurationContext context)
    {
        context.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();
  
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseAbpSecurityHeaders();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Common API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        AsyncHelper.RunSync(async () =>
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            }
        });
    }
}
