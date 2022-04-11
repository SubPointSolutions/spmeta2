using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class SPUserCustomActionExtensions
    {
        public static string GetRegistrationType(this SPUserCustomAction action)
        {
            return action.RegistrationType.ToString();
        }

    }
}
