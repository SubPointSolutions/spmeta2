using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class WebPartModels
    {
        #region properties

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

        public static WebPartDefinition OurLogoWebPart = new WebPartDefinition
        {
            Title = "Our Departments",
            Id = "appOurImage",
            WebpartType = typeof(ImageWebPart).AssemblyQualifiedName,
            ZoneId = "LeftColumn",
            ZoneIndex = 10
        };

        public static WebPartDefinition OurMembersWebPart = new WebPartDefinition
        {
            Title = "Our Members",
            Id = "appMembers",
            WebpartType = typeof(MembersWebPart).AssemblyQualifiedName,
            ZoneId = "LeftColumn",
            ZoneIndex = 10
        };



        #endregion
    }
}
