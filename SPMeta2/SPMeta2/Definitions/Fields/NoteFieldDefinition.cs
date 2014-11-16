using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Fields
{

    public static class BuiltInRichTextMode
    {
        #region properties

        public static string Compatible = "Compatible";
        public static string ThemeHtml = "ThemeHtml";
        public static string HtmlAsXml = "HtmlAsXml";
        public static string FullHtml = "FullHtml";

        #endregion
    }

    /// <summary>
    /// Allows to define and deploy note field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldMultiLineText", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldMultiLineText", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class NoteFieldDefinition : FieldDefinition
    {
        #region constructors

        public NoteFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Note;

            NumberOfLines = 6;
            RichTextMode = BuiltInRichTextMode.Compatible;
        }

        #endregion

        #region properties

        [ExpectValidation]
        public int NumberOfLines { get; set; }

        [ExpectValidation]
        public bool RichText { get; set; }

        [ExpectValidation]
        public string RichTextMode { get; set; }

        [ExpectValidation]
        public bool AppendOnly { get; set; }

        [ExpectValidation]
        public bool UnlimitedLengthInDocumentLibrary { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<NoteFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.NumberOfLines)
                          .AddPropertyValue(p => p.RichText)
                          .AddPropertyValue(p => p.RichTextMode)
                          .AddPropertyValue(p => p.AppendOnly)
                          .AddPropertyValue(p => p.UnlimitedLengthInDocumentLibrary)
                          .ToString();
        }

        #endregion
    }
}
