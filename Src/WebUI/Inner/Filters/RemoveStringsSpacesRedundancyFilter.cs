using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebUI.Inner.Attributes;

namespace WebUI.Inner.Filters
{
    public class RemoveStringsSpacesRedundancyFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool isPost = (string.Compare(context.HttpContext.Request.Method, HttpMethods.Post, StringComparison.OrdinalIgnoreCase) == 0);
            bool toLowerCase = !isPost;

            foreach (var key in context.ActionArguments.Keys.ToList())
            {
                var model = context.ActionArguments[key];
                context.ActionArguments[key] = ProcessModelStringProperties(model, toLowerCase);
            }
        }

		#region Helpers

		private static TSelf ProcessModelStringProperties<TSelf>(TSelf input, bool toLowerCase = false)
        {
            if (input == null)
                return input;

            if (input.GetType() == typeof(string))
            {
                object obj = TrimPropertyValue(input as string, toLowerCase);
                return (TSelf)obj;
            }

            var allProperties = input
                .GetType()
                .GetProperties()
                .Where(el => el.CanWrite && el.CanRead && el.GetGetMethod(true).IsPublic && el.GetSetMethod(true).IsPublic);

            foreach (var property in allProperties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string currentValue = (string)property.GetValue(input, null);

                    if (currentValue != null && currentValue.Length > 0)
                    {
                        if (property.GetCustomAttribute<PreserveStringSpacesAttribute>() == null)
                        {
                            currentValue = TrimPropertyValue(currentValue, toLowerCase);
                        }
                        else if (char.IsWhiteSpace(currentValue[0]) || char.IsWhiteSpace(currentValue[currentValue.Length - 1]))
                        {
                            // Don't trim HTML from within.
                            currentValue = currentValue.Trim();
                        }

                        property.SetValue(input, currentValue, null);
                    }
                }
            }

            return input;
        }

        private static string TrimPropertyValue(string value, bool toLowerCase)
        {
            if (value != null)
            {
                value = value.TrimWithin().TrimNewLines();

                if (value == string.Empty)
                    value = null;
                else if (toLowerCase)
                    value = value.ToLowerInvariant();
            }

            return value;
        }

        #endregion
    }
}
