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

        /// <summary>
        /// 'Cache Profiles' list shortcut.
        /// </summary>
        public static ListDefinition CacheProfiles = new ListDefinition
        {
            Title = "Cache Profiles",
            TemplateType = BuiltInListTemplateTypeId.GenericList,
            Url = "Cache Profiles",
            ContentTypesEnabled = true,
            Hidden = true
        };

        /// <summary>
        /// 'Composed Looks' list shortcut.
        /// </summary>
        public static ListDefinition ComposedLooks = new ListDefinition
        {
            Title = "Composed Looks",
            TemplateType = BuiltInListTemplateTypeId.GenericList,
            Url = "_catalogs/design",
            ContentTypesEnabled = true,
            Hidden = true
        };

        /// <summary>
        /// 'Device Channels' list shortcut.
        /// </summary>
        public static ListDefinition DeviceChannels = new ListDefinition
        {
            Title = "Composed Looks",
            TemplateType = BuiltInListTemplateTypeId.GenericList,
            Url = "DeviceChannels",
            ContentTypesEnabled = true,
            Hidden = true
        };

        /// <summary>
        /// 'Form Templates' list shortcut.
        /// </summary>
        public static ListDefinition FormTemplates = new ListDefinition
        {
            Title = "Form Templates",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
            Url = "FormServerTemplates",
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Publishing Images' list shortcut.
        /// </summary>
        public static ListDefinition Images = new ListDefinition
        {
            Title = "Images",
            TemplateType = 851,
            Url = "PublishingImages",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'List Template Gallery' list shortcut.
        /// </summary>
        public static ListDefinition ListTemplateGallery = new ListDefinition
        {
            Title = "List Template Gallery",
            TemplateType = 114,
            Url = "_catalogs/lt",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'Reusable Content' list shortcut.
        /// </summary>
        public static ListDefinition ReusableContent = new ListDefinition
        {
            Title = "Reusable Content",
            TemplateType = 100,
            //Url = "ReusableContent",
            CustomUrl = "ReusableContent",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'Site Collection Documents' list shortcut.
        /// </summary>
        public static ListDefinition SiteCollectionDocuments = new ListDefinition
        {
            Title = "Site Collection Documents",
            TemplateType = 101,
            Url = "SiteCollectionDocuments",
            ContentTypesEnabled = true,
        };


        /// <summary>
        /// 'Site Collection Images' list shortcut.
        /// </summary>
        public static ListDefinition SiteCollectionImages = new ListDefinition
        {
            Title = "Site Collection Images",
            TemplateType = 851,
            Url = "SiteCollectionImages",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'Suggested Content Browser Locations' list shortcut.
        /// </summary>
        public static ListDefinition SuggestedContentBrowserLocations = new ListDefinition
        {
            Title = "Suggested Content Browser Locations",
            TemplateType = 100,
            Url = "PublishedLinks",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'TaxonomyHiddenList' list shortcut.
        /// </summary>
        public static ListDefinition TaxonomyHiddenList = new ListDefinition
        {
            Title = "TaxonomyHiddenList",
            TemplateType = 100,
            Url = "TaxonomyHiddenList",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'Variation Labels' list shortcut.
        /// </summary>
        public static ListDefinition VariationLabels = new ListDefinition
        {
            Title = "Variation Labels",
            TemplateType = 100,
            Url = "Variation Labels",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'Workflow Tasks' list shortcut.
        /// </summary>
        public static ListDefinition WorkflowTasks = new ListDefinition
        {
            Title = "Workflow Tasks",
            TemplateType = 107,
            Url = "WorkflowTasks",
            ContentTypesEnabled = true,
        };

        #endregion

        #region catalogs


        /// <summary>
        /// Out of the box SharePoint list and libraries under "_catalogs" category.
        /// </summary>
        public static class Catalogs
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

        /// <summary>
        /// Obsolete, Please use Catalogs instead. 'Calalogs' has L typo and will be removed in future versions
        /// Out of the box SharePoint list and libraries under "_catalogs" category.
        /// </summary>
        [Obsolete("Please use Catalogs instead. 'Calalogs' has L typo and will be removed in future versions")]
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
