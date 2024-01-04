using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Data;
using Volo.Abp.Users;

namespace HQSOFT.eBiz.Main.AuditLogging
{
    public class ExtendedAuditLogContributor : AuditLogContributor
    {
        public override void PreContribute(AuditLogContributionContext context)
        {
            var url = context.AuditInfo.GetProperty("ScreenUrl");

            context.AuditInfo.SetProperty(
            "ScreenUrl",
                context.GetHttpContext().Request.Headers["screen-url"]
            );

        }

        public override void PostContribute(AuditLogContributionContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            foreach (var change in context.AuditInfo.EntityChanges)
            {
                change.SetProperty(
                "ScreenUrl",
                context.GetHttpContext().Request.Headers["screen-url"]);

                change.SetProperty(
                "UserId", currentUser.Id);
            }
        }
    }
}
