using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Enumerations;
using SPMeta2.Definitions;

namespace SPMeta2.BuiltInDefinitions
{
    /// <summary>
    /// Out of the box SharePoint list and libraries.
    /// </summary>
    public static class BuiltInListDefinitions
    {
        #region libraries

        /// <summary>
        /// 'Style Library' library shortcut.
        /// </summary>
        public static ListDefinition StyleLibrary = new ListDefinition
        {
            Title = "Style Library",
            Url = "Style Library",
            Description = "",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Site Pages' library shortcut.
        /// </summary>
        public static ListDefinition SitePages = new ListDefinition
        {
            Title = "Site Pages",
            TemplateType = BuiltInListTemplateTypeId.WebPageLibrary,
            Url = "SitePages",
            ContentTypesEnabled = false
        };

        /// <summary>
        /// 'Documents' library shortcut.
        /// </summary>
        public static ListDefinition Documents = new ListDefinition
        {
            Title = "Documents",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            Url = "Documents",
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Shared Documents' library shortcut.
        /// </summary>
        public static ListDefinition SharedDocuments = new ListDefinition
        {
            Title = "Documents",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            Url = "Shared Documents",
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Site Assets' library shortcut.
        /// </summary>
        public static ListDefinition SiteAssets = new ListDefinition
        {
            Title = "Site Assets",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            Url = "SiteAssets",
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Pages' library shortcut.
        /// </summary>
        public static ListDefinition Pages = new ListDefinition
        {
            Title = "Pages",
            TemplateType = 850,
            Url = "Pages",
            ContentTypesEnabled = true
        };

        #endregion

        #region lists


        #endregion

        #region catalogs

        /// <summary>
        /// Out of the box SharePoint list and libraries under "_catalogs" category.
        /// </summary>
        public static class Calalogs
        {
            public static ListDefinition AppData = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                Url = "_catalogs/appdata"
            };

            public static ListDefinition Design = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                Url = "_catalogs/design"
            };

            public static ListDefinition Users = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                Url = "_catalogs/users"
            };

            public static ListDefinition ListTemplates = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "_catalogs/lt"
            };

            public static ListDefinition MasterPage = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "_catalogs/masterpage",
            };

            public static ListDefinition Solutions = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "_catalogs/solutions"
            };

            public static ListDefinition Theme = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "_catalogs/theme"
            };

            public static ListDefinition WfPub = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "_catalogs/wfpub"
            };

            public static ListDefinition Wp = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "_catalogs/wp"
            };
        }

        #endregion
    }
}
