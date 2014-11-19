using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box field attributes.
    /// Used to generate XML for SharePoint field provision.
    /// </summary>
    public static class BuiltInFieldAttributes
    {
        #region properties

        public static string ID = "ID";
        public static string StaticName = "StaticName";
        public static string DisplayName = "DisplayName";
        public static string Title = "Title";
        public static string Name = "Name";
        public static string Type = "Type";
        public static string Group = "Group";

        public static string JSLink = "JSLink";
        //public static string DefaultValue = "DefaultValue";
        public static string Hidden = "Hidden";

        public static string ShowInDisplayForm = "ShowInDisplayForm";
        public static string ShowInEditForm = "ShowInEditForm";
        public static string ShowInListSettings = "ShowInListSettings";
        public static string ShowInNewForm = "ShowInNewForm";
        public static string ShowInVersionHistory = "ShowInVersionHistory";
        public static string ShowInViewForms = "ShowInViewForms";

        public static string AllowDeletion = "AllowDeletion";
        public static string Indexed = "Indexed";

        public static string MaxLength = "MaxLength";

        public static string ForcedDisplay = "ForcedDisplay";
        public static string Presence = "Presence";
        public static string UserSelectionScope = "UserSelectionScope";

        public static string UnlimitedLengthInDocumentLibrary = "UnlimitedLengthInDocumentLibrary";
        public static string RichText = "RichText";
        public static string RichTextMode = "RichTextMode";

        public static string NumLines = "NumLines";
        public static string AppendOnly = "AppendOnly";

        public static string LCID = "LCID";

        public static string FillInChoice = "FillInChoice";

        public static string SystemInstance = "SystemInstance";
        public static string EntityNamespace = "EntityNamespace";
        public static string EntityName = "EntityName";
        public static string BdcField = "BdcField";

        public static string Required = "Required";
        public static string Description = "Description";

        public static string CalType = "CalType";
        public static string FriendlyDisplayFormat = "FriendlyDisplayFormat";

        public static string UserSelectionMode = "UserSelectionMode";

        public static string Mult = "Mult";

        public static string List = "List";
        public static string WebId = "WebId";

        public static string ShowField = "ShowField";

        public static string Decimals = "Decimals";
        public static string Percentage = "Percentage";

        public static string Max = "Max";
        public static string Min = "Min";

        public static string EnableLookup = "EnableLookup";

        public static string Formula = "Formula";
        public static string ResultType = "ResultType";

        public static string Format = "Format";

        #endregion
    }
}
