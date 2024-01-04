using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace HQSOFT.Common.Blazor.Server;

[DependsOn(
    typeof(CommonBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
public class CommonBlazorServerModule : AbpModule
{

}
