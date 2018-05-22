using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows too define and apply SharePoint composed look item to a web.
    /// </summary>

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]


    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    //[ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]
    public class ComposedLookItemLinkDefinition : DefinitionBase
    {
        #region properties

        [DataMember]
        [IdentityKey]
        public string ComposedLookItemName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("ComposedLookItemName", ComposedLookItemName)
                          .ToString();
        }

        #endregion
    }
}
