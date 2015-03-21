using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.BuiltInDefinitions
{
    /// <summary>
    /// Out of the box SharePoint folders.
    /// </summary>
    public static class BuiltInFolderDefinitions
    {
        #region properties

        /// <summary>
        /// Forms folder in the libraries and lists.
        /// </summary>
        public static FolderDefinition Forms = new FolderDefinition
        {
            Name = "Forms"
        };

        public static FolderDefinition DisplayTemplates = new FolderDefinition
        {
            Name = "Display Templates"
        };

        public static FolderDefinition PreviewImages = new FolderDefinition
        {
            Name = "Preview Images"
        };

        public static FolderDefinition ContentWebParts = new FolderDefinition
        {
            Name = "Content Web Parts"
        };

        public static FolderDefinition Filters = new FolderDefinition
        {
            Name = "Filters"
        };

        public static FolderDefinition LanguageFiles = new FolderDefinition
        {
            Name = "Language Files"
        };

        public static FolderDefinition Search = new FolderDefinition
        {
            Name = "Search"
        };

        public static FolderDefinition ServerStyleSheets = new FolderDefinition
        {
            Name = "Server Style Sheets"
        };

        public static FolderDefinition System = new FolderDefinition
        {
            Name = "System"
        };

        #endregion
    }
}
