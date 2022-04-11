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
    /// Allows to install SharePoint application to the target web.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPAppInstance", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.AppInstance", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]

    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ParentHostCapability(typeof(WebDefinition))]
    public class AppDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// ProductId of the target application.
        /// </summary>
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public Guid ProductId { get; set; }


        /// <summary>
        /// Target application content.
        /// </summary>
        [DataMember]
        [IdentityKey]
        [ExpectRequired]
        public byte[] Content { get; set; }


        /// <summary>
        /// A valid Version string of the target application.
        /// </summary>
        [DataMember]
        [IdentityKey]
        [ExpectRequired]
        [VersionPropertyCapability]
        public string Version { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("ProductId", ProductId)
                          .AddRawPropertyValue("Version", Version)
                          .ToString();
        }

        #endregion
    }
}
