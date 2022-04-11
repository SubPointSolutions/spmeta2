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
    /// Allows too define and deploy Secure Store Application.
    /// </summary>

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.SecureStoreService.Server.ISecureStore", "Microsoft.Office.SecureStoreService")]
    //[SPObjectTypeAttribute(SPObjectModelType.CSOM, "", "")]


    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(FarmDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable] 
    [DataContract]

    [ParentHostCapability(typeof(FarmDefinition))]
    public class SecureStoreApplicationDefinition : DefinitionBase
    {
        #region properties

        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [DataMember]
        [IdentityKey]
        public Guid? Id { get; set; }

        [DataMember]
        [IdentityKey]
        public bool UseDefault { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Id", Id)
                          .AddRawPropertyValue("UseDefault", UseDefault)

                          .ToString();
        }

        #endregion
    }
}
