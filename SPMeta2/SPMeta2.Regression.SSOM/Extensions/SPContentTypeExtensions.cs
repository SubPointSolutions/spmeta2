using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class SPContentTypeExtensions
    {
        public static string GetId(this SPContentType c)
        {
            return c.Id.ToString();
        }
    }
}
