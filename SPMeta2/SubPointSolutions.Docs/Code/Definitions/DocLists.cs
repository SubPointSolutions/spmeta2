using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SubPointSolutions.Docs.Code.Definitions
{
    public static class DocLists
    {
        #region properties

        public static ListDefinition GeneralReports = new ListDefinition
        {
            Title = "General Reports",
            Description = "General reports register.",
            Url = "general-reports",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary
        };

        public static class HRLists
        {
            public static ListDefinition Poicies = new ListDefinition
            {
                Title = "Policies",
                Description = "Policies register.",
                Url = "policies",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary
            };

            public static ListDefinition Procedures = new ListDefinition
            {
                Title = "Procedures",
                Description = "Procedures register",
                Url = "procedures",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary
            };

            public static ListDefinition AnnualReviews = new ListDefinition
            {
                Title = "Annual reviews",
                Description = "Annual reviews.",
                Url = "procedures",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary
            };
        }

        public static class DepartmentsLists
        {
            public static ListDefinition TeamTasks = new ListDefinition
            {
                Title = "Team Tasks",
                Description = "Shared tasks for an IT team.",
                Url = "team-tasks",
                TemplateType = BuiltInListTemplateTypeId.Tasks
            };

            public static ListDefinition TeamEvents = new ListDefinition
            {
                Title = "Team Events",
                Description = "Shared events for an IT team.",
                Url = "team-events",
                TemplateType = BuiltInListTemplateTypeId.Events
            };

            public static ListDefinition IssueRegister = new ListDefinition
            {
                Title = "Issue Register",
                Description = "Shared events for an IT team.",
                Url = "shared-issues",
                TemplateType = BuiltInListTemplateTypeId.IssueTracking
            };
        }

        public static class AboutUsLists
        {
            public static ListDefinition OurClients = new ListDefinition
            {
                Title = "Our Clients",
                Description = "Client list.",
                Url = "our-clients",
                TemplateType = BuiltInListTemplateTypeId.Contacts
            };
            public static ListDefinition ManagementTeam = new ListDefinition
            {
                Title = "Management Team",
                Description = "Management team contacts.",
                Url = "management-team",
                TemplateType = BuiltInListTemplateTypeId.Contacts
            };
        }

        #endregion
    }
}
