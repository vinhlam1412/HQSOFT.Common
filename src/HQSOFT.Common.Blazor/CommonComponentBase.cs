using HQSOFT.Common.Localization;
using Volo.Abp.AspNetCore.Components;

namespace HQSOFT.Common.Blazor;

public abstract class CommonComponentBase : AbpComponentBase
{
    protected CommonComponentBase()
    {
        LocalizationResource = typeof(CommonResource);
    }
}
