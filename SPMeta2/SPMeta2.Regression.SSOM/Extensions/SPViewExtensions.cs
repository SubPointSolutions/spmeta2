using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class SPViewExtensions
    {
        public static string GetType(this ListViewDefinition def)
        {
            return def.Type.ToUpper();
        }

        public static string GetType(this SPView view)
        {
            return view.Type.ToUpper();
        }

        public static bool IsDefaul(this SPView view)
        {
            return view.ParentList.DefaultView.ID == view.ID;
        }

        public static string GetScope(this SPView view)
        {
            return view.Scope.ToString();
        }
    }
}
