using System.Collections.ObjectModel;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class SecurityRoleModels
    {
        #region properties

        public static SecurityRoleDefinition ContractorRole = new SecurityRoleDefinition
        {
            Name = "Contractor role",
            Description = "Specific control for contractors.",
            BasePermissions = new Collection<string>(new string[]
            {
                PermissionKind.ViewPages.ToString(),
                PermissionKind.EditListItems.ToString(),
                //PermissionKind.ManageLists.ToString(),
                PermissionKind.ApplyStyleSheets.ToString(),
                //PermissionKind.AddListItems.ToString(),
                //PermissionKind.CreateAlerts.ToString()
            })
        };

        public static SecurityRoleDefinition AnnouncmentsRole = new SecurityRoleDefinition
        {
            Name = "Announcements role",
            Description = "Can edit and publish.",
            BasePermissions = new Collection<string>(new string[]
            {
                PermissionKind.ViewPages.ToString(),
                //PermissionKind.EditListItems.ToString(),
                //PermissionKind.ManageLists.ToString(),
                //PermissionKind.DeleteVersions.ToString(),
                PermissionKind.ApplyStyleSheets.ToString(),
                //PermissionKind.AddListItems.ToString(),
                //PermissionKind.CreateAlerts.ToString()
            })
        };

        #endregion
    }
}
