using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to attach field to the target content type.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldLink", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldLink", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(SiteDefinition))]
    [DefaultParentHostAttribute(typeof(ContentTypeDefinition))]

    [Serializable]

    public class ContentTypeFieldLinkDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// ID of the target field to be attached to content type.
        /// BuiltInFieldId class can be used to utilize out of the box SharePoint fields. 
        /// </summary>
        public Guid FieldId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("FieldId:[{0}]", FieldId);
        }

        #endregion
    }
}
