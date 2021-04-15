using System.Threading;
using DotVVM.Framework.ViewModel;

namespace EventSourced.Diagnostics.Web.Pages
{
    public class ViewModelBase : DotvvmViewModelBase
    {
        public CancellationToken RequestCancellationToken => Context.GetCancellationToken();
    }
}