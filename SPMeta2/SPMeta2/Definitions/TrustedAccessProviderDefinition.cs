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
    /// Allows to define and deploy SharePoint web application.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.Claims.SPTrustedAccessProvider", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(FarmDefinition))]

    //[ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(FarmDefinition))]
    public class TrustedAccessProviderDefinition : DefinitionBase
    {
        #region constructors

        public TrustedAccessProviderDefinition()
        {
            Certificate = new byte[0];
        }

        #endregion

        #region properties


        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }


        [ExpectValidation]
        [DataMember]
        public string Description { get; set; }

        [ExpectValidation]
        [DataMember]
        public string MetadataEndPoint { get; set; }


        [ExpectValidation]
        [DataMember]
        [ExpectRequired]
        public byte[] Certificate { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .ToString();
        }

        #endregion
    }
}
