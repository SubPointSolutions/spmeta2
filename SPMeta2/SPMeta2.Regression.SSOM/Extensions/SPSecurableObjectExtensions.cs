using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class SPSecurableObjectExtensions
    {
        public static bool HasClearSubscopes(this SPSecurableObject secObject)
        {
            return secObject.RoleAssignments.Count == 0;
        }

        public static bool HasCopyRoleAssignments(this SPSecurableObject secObject)
        {
            var parent = secObject.FirstUniqueAncestorSecurableObject;

            return secObject.RoleAssignments == parent.RoleAssignments;
        }
    }
}
