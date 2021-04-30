using System.Threading;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Configuration;
using Microsoft.AspNetCore.Http;

namespace EventSourced.Diagnostics.Web.Pages
{
    public class ViewModelBase : DotvvmViewModelBase
    {
        private readonly EventSourcedDiagnosticsOptions _options;

        public ViewModelBase(EventSourcedDiagnosticsOptions options)
        {
            _options = options;
        }
        
        public CancellationToken RequestCancellationToken => Context.GetCancellationToken();

        public override async Task Init()
        {
            if (!await _options.Authorize(Context))
            {
                var response = Context.HttpContext.Response;
                response.StatusCode = 403;

                Context.InterruptRequest();
            }
            await base.Init();
        }
    }
}