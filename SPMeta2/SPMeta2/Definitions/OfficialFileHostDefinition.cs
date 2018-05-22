using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using SPMeta2.Enumerations;

namespace SPMeta2.Definitions
{

    /// <summary>
    /// Allows to deploy SPOfficialFileHost under target web application
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPOfficialFileHost", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [Serializable]
    [DataContract]

    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ParentHostCapability(typeof(WebApplicationDefinition))]
    public class OfficialFileHostDefinition : DefinitionBase
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        [ExpectValidation]
        public string OfficialFileName { get; set; }


        [DataMember]
        [ExpectRequired]
        [ExpectUpdateAsUrl]
        [ExpectValidation]
        public string OfficialFileUrl { get; set; }


        [DataMember]
        [ExpectUpdate]
        [ExpectValidation]
        public bool ShowOnSendToMenu { get; set; }

        [DataMember]
        [ExpectUpdate]
        [ExpectValidation]
        public string Explanation { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool CreateUniqueId { get; set; }

        /// <summary>
        /// Corresponds to SPOfficialFileHost.Action property
        /// </summary>
        [DataMember]
        [ExpectValidation]
        [ExpectUpdateAsIntRange(
            MinValue = (int)OfficialFileAction.Copy,
            MaxValue = (int)OfficialFileAction.Link)]
        public OfficialFileAction Action { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("OfficialFileName", OfficialFileName)
                          .AddRawPropertyValue("OfficialFileUrl", OfficialFileUrl)
                          .ToString();
        }

        #endregion
    }
}
