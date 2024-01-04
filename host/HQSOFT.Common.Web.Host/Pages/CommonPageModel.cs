using HQSOFT.Common.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace HQSOFT.Common.Pages;

public abstract class CommonPageModel : AbpPageModel
{
    protected CommonPageModel()
    {
        LocalizationResourceType = typeof(CommonResource);
    }
}
