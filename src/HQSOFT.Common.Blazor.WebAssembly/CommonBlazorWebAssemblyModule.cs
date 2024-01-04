using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace HQSOFT.Common.Blazor.WebAssembly;

[DependsOn(
    typeof(CommonBlazorModule),
    typeof(CommonHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
)]
public class CommonBlazorWebAssemblyModule : AbpModule
{

}
