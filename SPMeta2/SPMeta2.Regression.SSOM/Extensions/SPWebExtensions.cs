using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
   internal  static class SPWebExtensions
    {
        public static uint GetLCID(this SPWeb web)
        {
            return (uint)web.Locale.LCID;
        }

        public static string GetWebTemplate(this SPWeb web)
        {
            return string.Format("{0}#{1}", web.WebTemplate, web.Configuration);
        }
    }
}
