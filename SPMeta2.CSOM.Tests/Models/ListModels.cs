using Microsoft.SharePoint;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class ListModels
    {
        #region properties

        public static ListDefinition TestList = new ListDefinition
        {
            Title = "__Test Item List",
            Url = "TestItemList",
            TemplateType = 100,
            Description = "Test list to manage items",
            ContentTypesEnabled = true
        };

        public static ListDefinition TestLibrary = new ListDefinition
        {
            Title = "__Test Document Library",
            Url = "TestDocumentLibrary",
            TemplateType = 101,
            Description = "Test doc lib to manage docs",
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

        public static ListDefinition TestLinksList = new ListDefinition
        {
            Title = "__Links List",
            Url = "TestLinksList",
            TemplateName = "Links",
            Description = "Test links list",
            ContentTypesEnabled = true
        };

        public static ListDefinition SitePages = new ListDefinition
        {
            Title = "Site Pages",
            Url = "SitePages",
            TemplateType = 119,
            Description = "Use this library to create and store pages on this site.",
            ContentTypesEnabled = true
        };

        public static ListDefinition Pages = new ListDefinition
        {
            Title = "Pages",
            Url = "Pages",
            TemplateType = 119,
            Description = "Use this library to create and store pages on this site.",
            ContentTypesEnabled = true
        };

        #endregion
    }
}
