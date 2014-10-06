using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Definitions
{
    public static class RegWebs
    {
        #region properties

        public static WebDefinition Projects = new WebDefinition
        {
            Title = "Projects",
            Url = "Projects",
            Description = "Projects site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition Departments = new WebDefinition
        {
            Title = "Departments",
            Url = "Departments",
            Description = "Departments site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition HR = new WebDefinition
        {
            Title = "HR",
            Url = "HR",
            Description = "HR site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition IT = new WebDefinition
        {
            Title = "IT",
            Url = "IT",
            Description = "IT site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition Delivery = new WebDefinition
        {
            Title = "Delivery",
            Url = "Delivery",
            Description = "Delivery site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition Sales = new WebDefinition
        {
            Title = "Sales",
            Url = "Sales",
            Description = "Sales site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition PR = new WebDefinition
        {
            Title = "PR",
            Url = "PR",
            Description = "PR site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition CIO = new WebDefinition
        {
            Title = "CIO",
            Url = "CIO",
            Description = "CIO site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };

        public static WebDefinition Blog = new WebDefinition
        {
            Title = "Blog",
            Url = "Blog",
            Description = "Blog site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.Blog
        };

        public static WebDefinition CIOBlog = new WebDefinition
        {
            Title = "Blog",
            Url = "Blog",
            Description = "Blog site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.Blog
        };

        public static WebDefinition Wiki = new WebDefinition
        {
            Title = "Wiki",
            Url = "Wiki",
            Description = "Wiki site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.WikiSite
        };

        public static WebDefinition FAQ = new WebDefinition
        {
            Title = "FAQ",
            Url = "FAQ",
            Description = "FAQ site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.WikiSite
        };

        public static WebDefinition Archive = new WebDefinition
        {
            Title = "Archive",
            Url = "Archive",
            Description = "Archive site.",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };



        #endregion
    }
}
