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
    /// Allows to define SharePoint root web. Used to built up side model with root web provision included.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectAddHostExtensionMethod]
    [ExpectWithExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]
    public class RootWebDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [IdentityKey]
        [ExpectNullable]
        public string Title { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                         .ToString();
        }

        #endregion
    }
}
