using HQSOFT.Common.Localization;
using Volo.Abp.Application.Services;

namespace HQSOFT.Common;

public abstract class CommonAppService : ApplicationService
{
    protected CommonAppService()
    {
        LocalizationResource = typeof(CommonResource);
        ObjectMapperContext = typeof(CommonApplicationModule);
    }
}
