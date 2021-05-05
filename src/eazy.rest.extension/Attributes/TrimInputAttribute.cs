using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eazy.rest.extension.Attributes
{
    /// <summary>
    ///     Return a new string in which all leading and trailing occurrences of a set of specified characters from the current
    ///     string are remover
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class TrimInputAttribute : ActionFilterAttribute
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

                TrimAllStringsInObject(arg.Value, argType);
            }
        }

        private void TrimAllStringsInObject(object arg, Type argType)
        {
            var stringProperties = argType.GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                var currentValue = stringProperty.GetValue(arg, null) as string;
                if (!string.IsNullOrEmpty(currentValue)) stringProperty.SetValue(arg, currentValue.Trim(), null);
            }
        }
    }
}