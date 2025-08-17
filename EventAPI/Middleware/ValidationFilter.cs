using EventAPI.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;

namespace EventAPI.Middleware
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new DomainValidationException(errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
