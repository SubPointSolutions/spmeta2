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
    /// Allows to define and deploy SharePoint web site locale.
    /// </summary>

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]


    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    //[ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]

    [ExpectManyInstances]
    public class SupportedUICultureDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public int LCID { get; set; }


        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("LCID", LCID)
                          .ToString();
        }

        #endregion
    }
}
