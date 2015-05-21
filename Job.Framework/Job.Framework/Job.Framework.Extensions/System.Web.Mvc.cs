using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class MvcExtensions
    {
        public static IDictionary<string, object> MergeHtmlAttributes<TModel>(this HtmlHelper<TModel> htmlHelper, object htmlAttributes = null)
        {
            var attributes = htmlHelper.ViewData.ContainsKey("htmlAttributes") ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlHelper.ViewData["htmlAttributes"]) : new RouteValueDictionary();
             
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    var key = property.Name.Replace('_', '-');

                    if (!attributes.ContainsKey(key))
                    {
                        attributes.Add(key, property.GetValue(htmlAttributes));
                    }
                }
            }

            return attributes;
        }
    }
}
