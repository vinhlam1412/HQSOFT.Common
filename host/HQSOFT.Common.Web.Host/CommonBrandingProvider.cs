using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace HQSOFT.Common;

[Dependency(ReplaceServices = true)]
public class CommonBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Common";
}
