using SPMeta2.Definitions;
using SPMeta2.Enumerations;

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
#pragma warning disable 618
            Url = "Style Library",
#pragma warning restore 618
            CustomUrl = "Style Library",
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
#pragma warning disable 618
            Url = "SitePages",
#pragma warning restore 618
            CustomUrl = "SitePages",
            ContentTypesEnabled = false
        };

        /// <summary>
        /// 'Documents' library shortcut.
        /// </summary>
        public static ListDefinition Documents = new ListDefinition
        {
            Title = "Documents",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
#pragma warning disable 618
            Url = "Documents",
#pragma warning restore 618
            CustomUrl = "Documents",
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Shared Documents' library shortcut.
        /// </summary>
        public static ListDefinition SharedDocuments = new ListDefinition
        {
            Title = "Documents",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
#pragma warning disable 618
            Url = "Shared Documents",
#pragma warning restore 618
            CustomUrl = "Shared Documents",
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Site Assets' library shortcut.
        /// </summary>
        public static ListDefinition SiteAssets = new ListDefinition
        {
            Title = "Site Assets",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
#pragma warning disable 618
            Url = "SiteAssets",
#pragma warning restore 618
            CustomUrl = "SiteAssets",
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Pages' library shortcut.
        /// </summary>
        public static ListDefinition Pages = new ListDefinition
        {
            Title = "Pages",
            TemplateType = 850,
#pragma warning disable 618
            Url = "Pages",
#pragma warning restore 618
            CustomUrl = "Pages",
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
#pragma warning disable 618
            Url = "Cache Profiles",
#pragma warning restore 618
            CustomUrl = "Cache Profiles",
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
#pragma warning disable 618
            Url = "_catalogs/design",
#pragma warning restore 618
            CustomUrl = "_catalogs/design",
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
#pragma warning disable 618
            Url = "DeviceChannels",
#pragma warning restore 618
            CustomUrl = "DeviceChannels",
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
#pragma warning disable 618
            Url = "FormServerTemplates",
#pragma warning restore 618
            CustomUrl = "FormServerTemplates",
            ContentTypesEnabled = true
        };

        /// <summary>
        /// 'Publishing Images' list shortcut.
        /// </summary>
        public static ListDefinition Images = new ListDefinition
        {
            Title = "Images",
            TemplateType = 851,
#pragma warning disable 618
            Url = "PublishingImages",
#pragma warning restore 618
            CustomUrl = "PublishingImages",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'List Template Gallery' list shortcut.
        /// </summary>
        public static ListDefinition ListTemplateGallery = new ListDefinition
        {
            Title = "List Template Gallery",
            TemplateType = 114,
#pragma warning disable 618
            Url = "_catalogs/lt",
#pragma warning restore 618
            CustomUrl = "_catalogs/lt",
            ContentTypesEnabled = false,
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
#pragma warning disable 618
            Url = "SiteCollectionDocuments",
#pragma warning restore 618
            CustomUrl = "SiteCollectionDocuments",
            ContentTypesEnabled = true,
        };


        /// <summary>
        /// 'Site Collection Images' list shortcut.
        /// </summary>
        public static ListDefinition SiteCollectionImages = new ListDefinition
        {
            Title = "Site Collection Images",
            TemplateType = 851,
#pragma warning disable 618
            Url = "SiteCollectionImages",
#pragma warning restore 618
            CustomUrl = "SiteCollectionImages",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'Suggested Content Browser Locations' list shortcut.
        /// </summary>
        public static ListDefinition SuggestedContentBrowserLocations = new ListDefinition
        {
            Title = "Suggested Content Browser Locations",
            TemplateType = 100,
#pragma warning disable 618
            Url = "PublishedLinks",
#pragma warning restore 618
            CustomUrl = "PublishedLinks",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'TaxonomyHiddenList' list shortcut.
        /// </summary>
        public static ListDefinition TaxonomyHiddenList = new ListDefinition
        {
            Title = "TaxonomyHiddenList",
            TemplateType = 100,
#pragma warning disable 618
            Url = "TaxonomyHiddenList",
#pragma warning restore 618
            CustomUrl = "Lists/TaxonomyHiddenList",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'Variation Labels' list shortcut.
        /// </summary>
        public static ListDefinition VariationLabels = new ListDefinition
        {
            Title = "Variation Labels",
            TemplateType = 100,
#pragma warning disable 618
            Url = "Variation Labels",
#pragma warning restore 618
            CustomUrl = "Variation Labels",
            ContentTypesEnabled = true,
        };

        /// <summary>
        /// 'Workflow Tasks' list shortcut.
        /// </summary>
        public static ListDefinition WorkflowTasks = new ListDefinition
        {
            Title = "Workflow Tasks",
            TemplateType = 107,
#pragma warning disable 618
            Url = "WorkflowTasks",
#pragma warning restore 618
            CustomUrl = "WorkflowTasks",
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
#pragma warning disable 618
                Url = "_catalogs/appdata",
#pragma warning restore 618
                CustomUrl = "_catalogs/appdata",
            };

            public static ListDefinition Design = new ListDefinition
            {
                Title = "Composed Looks",
                TemplateType = BuiltInListTemplateTypeId.DesignCatalog,
#pragma warning disable 618
                Url = "_catalogs/design",
#pragma warning restore 618
                CustomUrl = "_catalogs/design",
                ContentTypesEnabled = false,
                Hidden = false
            };

            public static ListDefinition Users = new ListDefinition
            {
                Title = "User Information List",
                TemplateType = BuiltInListTemplateTypeId.UserInformation,
#pragma warning disable 618
                Url = "_catalogs/users",
#pragma warning restore 618
                CustomUrl = "_catalogs/users",
            };

            public static ListDefinition ListTemplates = new ListDefinition
            {
                Title = "List Template Gallery",
                TemplateType = BuiltInListTemplateTypeId.ListTemplateCatalog,
#pragma warning disable 618
                Url = "_catalogs/lt",
#pragma warning restore 618
                CustomUrl = "_catalogs/lt",
            };

            public static ListDefinition MasterPage = new ListDefinition
            {
                Title = "Master Page Gallery",
                TemplateType = BuiltInListTemplateTypeId.MasterPageCatalog,
#pragma warning disable 618
                Url = "_catalogs/masterpage",
#pragma warning restore 618
                CustomUrl = "_catalogs/masterpage",
            };

            public static ListDefinition Solutions = new ListDefinition
            {
                Title = "Solution Gallery",
                TemplateType = BuiltInListTemplateTypeId.SolutionCatalog,
#pragma warning disable 618
                Url = "_catalogs/solutions",
#pragma warning restore 618
                CustomUrl = "_catalogs/solutions"
            };

            public static ListDefinition Theme = new ListDefinition
            {
                Title = "Theme Gallery",
                TemplateType = BuiltInListTemplateTypeId.ThemeCatalog,
#pragma warning disable 618
                Url = "_catalogs/theme",
#pragma warning restore 618
                CustomUrl = "_catalogs/theme"
            };

            public static ListDefinition WfPub = new ListDefinition
            {
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
#pragma warning disable 618
                Url = "_catalogs/wfpub",
#pragma warning restore 618
                CustomUrl = "_catalogs/wfpub"
            };

            public static ListDefinition Wp = new ListDefinition
            {
                Title = "Web Part Gallery",
                TemplateType = BuiltInListTemplateTypeId.WebPartCatalog,
#pragma warning disable 618
                Url = "_catalogs/wp",
#pragma warning restore 618
                CustomUrl = "_catalogs/wp"
            };
        }

        #endregion
    }
}
