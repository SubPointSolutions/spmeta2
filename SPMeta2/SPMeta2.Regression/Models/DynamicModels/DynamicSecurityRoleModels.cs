using System;
using System.Collections.ObjectModel;
using SPMeta2.Definitions;
using SPMeta2.Regression.Const;

namespace SPMeta2.Regression.Models.DynamicModels
{
    public static class DynamicSecurityRoleModels
    {
        public static SecurityRoleDefinition ContractorRole = GetSecurityRoleTestTemplate("Contractor", role =>
        {
            role.BasePermissions = new Collection<string>(new[]{
                                           SPBasePermissions.ViewPages.ToString(),
                                           SPBasePermissions.EditListItems.ToString(),
                                           SPBasePermissions.ManageLists.ToString()
                                       });
        });

        public static SecurityRoleDefinition WorkerRole = GetSecurityRoleTestTemplate("Worker", role =>
        {
            role.BasePermissions =new Collection<string>( new[]{
                                           SPBasePermissions.ViewPages.ToString(),
                                           SPBasePermissions.ManageLists.ToString()
                                       });
        });

        public static SecurityRoleDefinition StudentRole = GetSecurityRoleTestTemplate("Student", role =>
        {
            role.BasePermissions = new Collection<string>(new[]{
                                           SPBasePermissions.ApplyStyleSheets.ToString(),
                                           SPBasePermissions.BrowseUserInfo.ToString(),
                                           SPBasePermissions.ManageLists.ToString()
                                       });
        });

        #region methods

        public static SecurityRoleDefinition GetSecurityRoleTestTemplate(string groupRolePrefix, Action<SecurityRoleDefinition> action)
        {
            var result = new SecurityRoleDefinition
            {
                Name = string.Format("{0}Role{1}", groupRolePrefix, Environment.TickCount),
                Description = string.Format("{0}description{1}", groupRolePrefix, Environment.TickCount)
            };

            if (action != null) action(result);

            return result;
        }

        #endregion
    }

}
