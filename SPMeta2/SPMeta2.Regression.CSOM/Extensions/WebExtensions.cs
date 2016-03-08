using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Regression.CSOM.Extensions
{
    internal static class WebExtensions
    {
        public static uint GetLCID(this Web web)
        {
            return (uint)web.Language;
        }

        public static string GetWebTemplate(this Web web)
        {
            return string.Format("{0}#{1}", web.WebTemplate, web.Configuration);
        }
    }
}
