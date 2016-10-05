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
    /// Allows to consigure SPDeveloperDashboardSettings.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPDeveloperDashboardSettings", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(FarmDefinition))]

    [Serializable]
    [DataContract]

    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    [SelfHostCapability]

    [ParentHostCapability(typeof(FarmDefinition))]
    public class DeveloperDashboardSettingsDefinition : DefinitionBase
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
            return new ToStringResult<DeveloperDashboardSettingsDefinition>(this)
                          .AddPropertyValue(p => p.ProductId)
                          .AddPropertyValue(p => p.Version)
                          .ToString();
        }

        #endregion
    }
}
