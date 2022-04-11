using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class SPRoleAssignmentExtensions
    {
        public static string GetRoleDefinitionBindings(this SPRoleAssignment assignment)
        {
            return assignment.RoleDefinitionBindings.ToString();
        }
    }
}
