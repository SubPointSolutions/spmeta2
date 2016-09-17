using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SubPointSolutions.Docs.Code.Definitions
{
    public static class DocWebs
    {
        #region properties

        public static WebDefinition News = new WebDefinition
        {
            Title = "News",
            Description = "A news sites.",
            Url = "news",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition AboutOurCompany = new WebDefinition
        {
            Title = "About Our Company",
            Description = "A big story about us.",
            Url = "about-our-company",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition Departments = new WebDefinition
        {
            Title = "Departments",
            Description = "Top level web for all departments.",
            Url = "departments",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static class DepartmentWebs
        {
            public static WebDefinition HR = new WebDefinition
            {
                Title = "HR",
                Description = "HR specific web.",
                Url = "hr",
                WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
            };

            public static WebDefinition ITHelpDesk = new WebDefinition
            {
                Title = "IT HelpDesk",
                Description = "Solving IT issues.",
                Url = "it-helpdesk",
                WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
            };

            public static class ITHelpDeskWebs
            {
                public static WebDefinition Microsoft = new WebDefinition
                {
                    Title = "Microsoft",
                    Description = "Solving Microsoft related IT issues.",
                    Url = "microsoft",
                    WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
                };

                public static WebDefinition Cisco = new WebDefinition
                {
                    Title = "Cisco",
                    Description = "Solving Cisco related IT issues.",
                    Url = "cisco",
                    WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
                };

                public static WebDefinition Apple = new WebDefinition
                {
                    Title = "Apple",
                    Description = "Solving Apple related IT issues.",
                    Url = "apple",
                    WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
                };
            }


            public static WebDefinition Sales = new WebDefinition
            {
                Title = "Sales",
                Description = "Sales team collaboration and documents.",
                Url = "sales",
                WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
            };
        }

        #endregion
    }
}
