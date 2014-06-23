using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Enumerations;
using SPMeta2.Definitions;

namespace SPMeta2.BuiltInDefinitions
{
    public static class BuiltInListDefinitions
    {
        #region libraries

        public static ListDefinition StyleLibrary = new ListDefinition
        {
            Title = "Style Library",
            Url = "Style Library",
            Description = "",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            ContentTypesEnabled = true
        };

        public static ListDefinition SitePages = new ListDefinition
        {
            Title = "Site Pages",
            TemplateType = BuiltInListTemplateTypeId.WebPageLibrary,
            Url = "SitePages",
            ContentTypesEnabled = false
        };

        public static ListDefinition Documents = new ListDefinition
        {
            Title = "Documents",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            Url = "Shared Documents",
            ContentTypesEnabled = true
        };

        public static ListDefinition SiteAssets = new ListDefinition
        {
            Title = "Site Assets",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            Url = "SiteAssets",
            ContentTypesEnabled = true
        };

        public static ListDefinition Pages = new ListDefinition
        {
            Title = "Pages",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            Url = "Pages",
            ContentTypesEnabled = true
        };

        #endregion

        #region lists


        #endregion
    }
}
