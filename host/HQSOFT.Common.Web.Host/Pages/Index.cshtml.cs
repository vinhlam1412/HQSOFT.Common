using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace HQSOFT.Common.Pages;

public class IndexModel : CommonPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
