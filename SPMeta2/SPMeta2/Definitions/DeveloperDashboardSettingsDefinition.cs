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
    /// Allows to consigure SPDeveloperDashboardSettings
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPDeveloperDashboardSettings", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(FarmDefinition))]

    [Serializable]
    [DataContract]

    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    //[SelfHostCapability]
    [SingletonIdentity]

    [ParentHostCapability(typeof(FarmDefinition))]
    public class DeveloperDashboardSettingsDefinition : DefinitionBase
    {
        #region properties

        [DataMember]
        [ExpectUpdateDeveloperDashboardSettings]
        [ExpectValidation]
        [ExpectNullable]
        public string DisplayLevel { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("DisplayLevel", DisplayLevel)
                          .ToString();
        }

        #endregion
    }
}
