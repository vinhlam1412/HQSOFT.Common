using Abp.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.DependencyInjection;
using Microsoft.AspNetCore.Components;

namespace HQSOFT.Common.AuditLogging
{
    public class AuditMessageHandler :DelegatingHandler, ITransientDependency
    {
        private readonly NavigationManager _navigationManager;

        public AuditMessageHandler(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Add("screen-url", _navigationManager.Uri);
            return base.SendAsync(request, cancellationToken);
        }
    }

}
