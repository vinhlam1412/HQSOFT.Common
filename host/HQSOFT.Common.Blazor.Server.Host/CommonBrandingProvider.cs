using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace HQSOFT.Common.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class CommonBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Common";
}
