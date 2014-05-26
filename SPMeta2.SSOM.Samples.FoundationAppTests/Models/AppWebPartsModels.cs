using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppWebPartsModels
    {
        #region properties

        public static class RootWeb
        {
            public static class SitePages
            {
                public static class AboutFounationAppPage
                {
                    public static WebPartDefinition AboutUsWebPart = new WebPartDefinition
                    {
                        Title = "About Us",
                        Id = "appAboutUsWebPart",
                        WebpartType = typeof(ContentEditorWebPart).AssemblyQualifiedName,
                        ZoneId = "Header",
                        ZoneIndex = 10
                    };

                    public static WebPartDefinition OurValuesWebPart = new WebPartDefinition
                    {
                        Title = "Our Values",
                        Id = "appOurValuesWebPart",
                        WebpartType = typeof(ContentEditorWebPart).AssemblyQualifiedName,
                        ZoneId = "LeftColumn",
                        ZoneIndex = 10
                    };

                    public static WebPartDefinition OurServicesWebPart = new WebPartDefinition
                    {
                        Title = "Our Services",
                        Id = "appOurServicesWebPart",
                        WebpartType = typeof(ContentEditorWebPart).AssemblyQualifiedName,
                        ZoneId = "MiddleColumn",
                        ZoneIndex = 10
                    };

                    public static WebPartDefinition OurDepartmentsWebPart = new WebPartDefinition
                    {
                        Title = "Our Departments",
                        Id = "appOurDepartmentsWebPart",
                        WebpartType = typeof(XsltListViewWebPart).AssemblyQualifiedName,
                        ZoneId = "MiddleColumn",
                        ZoneIndex = 20
                    };

                    public static WebPartDefinition ContactUsWebPart = new WebPartDefinition
                    {
                        Title = "Contact Us",
                        Id = "appContactUsWebPart",
                        WebpartType = typeof(SimpleFormWebPart).AssemblyQualifiedName,
                        ZoneId = "RightColumn",
                        ZoneIndex = 10
                    };
                }
            }
        }

        public static class AboutFounationAppPage
        {
            public static WebPartDefinition AboutUsWebPart = new WebPartDefinition
            {
                Title = "About Us",
                Id = "appAboutUsWebPart",
                WebpartType = typeof(ContentEditorWebPart).AssemblyQualifiedName,
                ZoneId = "Header",
                ZoneIndex = 10
            };

            public static WebPartDefinition OurValuesWebPart = new WebPartDefinition
            {
                Title = "Our Values",
                Id = "appOurValuesWebPart",
                WebpartType = typeof(ContentEditorWebPart).AssemblyQualifiedName,
                ZoneId = "LeftColumn",
                ZoneIndex = 10
            };

            public static WebPartDefinition OurServicesWebPart = new WebPartDefinition
            {
                Title = "Our Services",
                Id = "appOurServicesWebPart",
                WebpartType = typeof(ContentEditorWebPart).AssemblyQualifiedName,
                ZoneId = "MiddleColumn",
                ZoneIndex = 10
            };

            public static WebPartDefinition OurDepartmentsWebPart = new WebPartDefinition
            {
                Title = "Our Departments",
                Id = "appOurDepartmentsWebPart",
                WebpartType = typeof(XsltListViewWebPart).AssemblyQualifiedName,
                ZoneId = "MiddleColumn",
                ZoneIndex = 20
            };

            public static WebPartDefinition ContactUsWebPart = new WebPartDefinition
            {
                Title = "Contact Us",
                Id = "appContactUsWebPart",
                WebpartType = typeof(SimpleFormWebPart).AssemblyQualifiedName,
                ZoneId = "RightColumn",
                ZoneIndex = 10
            };
        }


        #endregion
    }
}
