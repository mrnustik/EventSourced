using System.Threading;
using DotVVM.Framework.ViewModel;

namespace EventSourced.Sample.Warehouse.Web.Pages
{
    public class ViewModelBase : DotvvmViewModelBase
    {
        protected CancellationToken RequestCancellationToken => Context.GetCancellationToken();
    }
}