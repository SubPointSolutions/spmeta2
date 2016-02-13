using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;

namespace SPMeta2.Regression.CSOM.Extensions
{
    internal static class SecurableObjectExtensions
    {
        public static bool HasClearSubscopes(this SecurableObject secObject)
        {
            if (!secObject.IsPropertyAvailable("RoleAssignments"))
            {
                secObject.Context.Load(secObject, s => s.RoleAssignments);
                secObject.Context.ExecuteQueryWithTrace();
            }

            return secObject.RoleAssignments.Count == 0;
        }

        public static bool HasCopyRoleAssignments(this SecurableObject secObject)
        {
            if (!secObject.IsPropertyAvailable("FirstUniqueAncestorSecurableObject"))
            {
                secObject.Context.Load(secObject, s => s.FirstUniqueAncestorSecurableObject);
                secObject.Context.ExecuteQueryWithTrace();
            }

            var parent = secObject.FirstUniqueAncestorSecurableObject;
            return secObject.RoleAssignments == parent.RoleAssignments;
        }
    }
}
