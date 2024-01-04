using HQSOFT.Common.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace HQSOFT.Common;

public abstract class CommonController : AbpControllerBase
{
    protected CommonController()
    {
        LocalizationResource = typeof(CommonResource);
    }
}
