using Microsoft.SharePoint;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppListModels
    {
        #region properties

        public static ListDefinition CompanyAnnouncements = new ListDefinition
        {
            Title = "Company Announcements",
            Url = "Company Announcements",
            Description = "Stores company announcements for the landing page",
            TemplateType = (int)SPListTemplateType.Announcements,
            ContentTypesEnabled = true
        };

        public static ListDefinition SitePages = new ListDefinition
        {
            Title = "Site Pages",
            Url = "SitePages",
            Description = "Stores company announcements for the landing page",
            TemplateType = (int)SPListTemplateType.DocumentLibrary,
            ContentTypesEnabled = true
        };

        public static ListDefinition StyleLibrary = new ListDefinition
        {
            Title = "Style Library",
            Url = "Style Library",
            Description = "",
            TemplateType = (int)SPListTemplateType.DocumentLibrary,
            ContentTypesEnabled = true
        };

        public static ListDefinition CompanyDepartments = new ListDefinition
        {
            Title = "Company Departments",
            Url = "Company Departments",
            Description = "List of all company departments",
            TemplateType = (int)SPListTemplateType.GenericList,
            ContentTypesEnabled = true,
            NeedToCopyRoleAssignmets = true
        };

        public static ListDefinition DepartmentTasks = new ListDefinition
        {
            Title = "Department Tasks",
            Url = "Department Tasks",
            Description = "Task with a 'department' scope. Meant to be taken by any free person in the target department.",
            TemplateType = (int)SPListTemplateType.Tasks,
            ContentTypesEnabled = true
        };

        #endregion
    }
}
