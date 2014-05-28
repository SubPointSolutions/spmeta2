using System.Collections.ObjectModel;
using Microsoft.SharePoint;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppSecurityRoleModels
    {
        #region properties

        public static SecurityRoleDefinition ContractorRole = new SecurityRoleDefinition
        {
            Name = "Contractor role",
            Description = "Specific control for contractors.",
            BasePermissions = new Collection<string>(new string[]
            {
                SPBasePermissions.ViewPages.ToString(),
                SPBasePermissions.EditListItems.ToString(),
                SPBasePermissions.ManageLists.ToString()
            })
        };

        public static SecurityRoleDefinition AnnouncmentsRole = new SecurityRoleDefinition
        {
            Name = "Announcements role",
            Description = "Can edit and publish.",
            BasePermissions = new Collection<string>(new string[]
            {
                SPBasePermissions.ViewPages.ToString(),
                SPBasePermissions.EditListItems.ToString(),
                SPBasePermissions.ManageLists.ToString()
            })
        };

        #endregion
    }
}
