using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegSecurityRoles
    {
        #region properties

        public static SecurityRoleDefinition SecurityRole1 = new SecurityRoleDefinition
        {
            Name = "Security Role 1",
            Description = "Specific role 1.",
            BasePermissions = new Collection<string>(new string[]
            {
                "ViewPages",
                "EditListItems"
            })
        };

        public static SecurityRoleDefinition SecurityRole2 = new SecurityRoleDefinition
        {
            Name = "Security Role 2",
            Description = "Specific role 2.",
            BasePermissions = new Collection<string>(new string[]
            {
                "ViewPages",
                "EditListItems",
                "ManageLists"
            })
        };

        #endregion
    }
}
