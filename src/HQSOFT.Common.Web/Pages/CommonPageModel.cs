using HQSOFT.Common.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace HQSOFT.Common.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class CommonPageModel : AbpPageModel
{
    protected CommonPageModel()
    {
        LocalizationResourceType = typeof(CommonResource);
        ObjectMapperContext = typeof(CommonWebModule);
    }
}
