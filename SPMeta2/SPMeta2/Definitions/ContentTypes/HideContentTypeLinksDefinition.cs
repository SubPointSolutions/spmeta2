using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.ContentTypes
{
    /// <summary>
    /// Allows to hide particular content types in the target list.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFolder", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Folder", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(ListDefinition))]
    public class HideContentTypeLinksDefinition : DefinitionBase
    {
        #region constructors

        public HideContentTypeLinksDefinition()
        {
            ContentTypes = new List<ContentTypeLinkValue>();
        }

        #endregion

        #region properties

        [DataMember]
        [IdentityKey]
        [ExpectValidation]
        public List<ContentTypeLinkValue> ContentTypes { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("ContentTypes", ContentTypes)
                          .ToString();
        }

        #endregion
    }
}
