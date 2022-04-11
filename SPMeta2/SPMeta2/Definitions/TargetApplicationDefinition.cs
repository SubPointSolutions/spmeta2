using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [DataContract]
    public class TargetApplicationFieldValue
    {
        [DataMember]
        public bool IsMasked { get; set; }


        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CredentialType { get; set; }
    }

    /// <summary>
    /// Allows to define and deploy secure store target applicationL.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.SecureStoreService.Server.TargetApplication", "Microsoft.Office.SecureStoreService")]

    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(SecureStoreApplicationDefinition))]

    [Serializable]
    [DataContract]

    [ExpectWithExtensionMethod]

    [ParentHostCapability(typeof(SecureStoreApplicationDefinition))]
    public class TargetApplicationDefinition : DefinitionBase
    {
        #region constructors

        public TargetApplicationDefinition()
        {
            TargetApplicationClams = new Collection<string>();
            Fields = new Collection<TargetApplicationFieldValue>();
        }

        #endregion

        #region properties

        [DataMember]
        [IdentityKey]
        public string ApplicationId { get; set; }

        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public string FriendlyName { get; set; }


        [DataMember]
        public string ContactEmail { get; set; }

        [DataMember]
        public int TicketTimeout { get; set; }


        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public Collection<string> TargetApplicationClams { get; set; }

        [DataMember]
        public Collection<TargetApplicationFieldValue> Fields { get; set; }

        [DataMember]
        public string CredentialManagementUrl { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("ApplicationId", ApplicationId)
                          .AddRawPropertyValue("FriendlyName", FriendlyName)
                          .AddRawPropertyValue("Type", Type)

                          .ToString();
        }

        #endregion
    }
}
