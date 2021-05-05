using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eazy.rest.extension.Attributes
{
    /// <summary>
    ///     Returns a copy of this String object converted to lowercase using the casing rules of the invariant culture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class LowerInputAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var arg in context.ActionArguments)
            {
                if (arg.Value is string)
                {
                    var val = arg.Value as string;
                    if (!string.IsNullOrEmpty(val)) context.ActionArguments[arg.Key] = val.Trim();

                    continue;
                }

                var argType = arg.Value.GetType();
                if (!argType.IsClass) continue;


                LowerAllStringsInObject(arg.Value, argType);
            }
        }

        private void LowerAllStringsInObject(object arg, Type argType)
        {
            var stringProperties = argType.GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                var currentValue = stringProperty.GetValue(arg, null) as string;
                if (!string.IsNullOrEmpty(currentValue))
                    stringProperty.SetValue(arg, currentValue.ToLowerInvariant(), null);
            }
        }
    }
}