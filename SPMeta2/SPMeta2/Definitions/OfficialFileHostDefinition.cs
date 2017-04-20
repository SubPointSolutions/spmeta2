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
    /// Allows to deploy SPOfficialFileHost under target web application
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPOfficialFileHost ", "Microsoft.SharePoint")]

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
        public string OfficialFileName { get; set; }


        [DataMember]
        [ExpectRequired]
        public string OfficialFileUrl { get; set; }


        [DataMember]
        public bool ShowOnSendToMenu { get; set; }

        [DataMember]
        public string Explanation { get; set; }


        [DataMember]
        public Guid? UniqueId { get; set; }

        /// <summary>
        /// Corresponds to SPOfficialFileHost.Action property
        /// </summary>
        [DataMember]
        public string Action { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<OfficialFileHostDefinition>(this)
                          .AddPropertyValue(p => p.OfficialFileName)
                          .AddPropertyValue(p => p.OfficialFileUrl)
                          .ToString();
        }

        #endregion
    }
}
