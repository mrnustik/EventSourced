using System;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel.Validation;
using EventSourced.Sample.Warehouse.Domain.Exceptions;

namespace EventSourced.Sample.Warehouse.Web
{
    public class BusinessRuleExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public string ErrorItemPath { get; }

        public BusinessRuleExceptionFilterAttribute(string errorItemPath)
        {
            ErrorItemPath = errorItemPath;
        }

        protected override Task OnCommandExceptionAsync(IDotvvmRequestContext context, ActionInfo actionInfo, Exception ex)
        {
            if (ex is BusinessRuleException businessRuleException)
            {
                context.ModelState.Errors.Add(new ViewModelValidationError
                {
                    ErrorMessage = businessRuleException.Message,
                    PropertyPath = ErrorItemPath,
                });
                context.IsCommandExceptionHandled = true;
                
                context.FailOnInvalidModelState();
            }
            return base.OnCommandExceptionAsync(context, actionInfo, ex);
        }
    }
}