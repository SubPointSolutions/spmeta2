using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using SPMeta2.Attributes.Identity;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint wiki page.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPUser", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.User", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SecurityGroupDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SecurityGroupDefinition))]

    [ExpectManyInstances]

    public class UserDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [ExpectRequired(GroupName = "LoginName or Email")]
        public string LoginName { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [ExpectRequired(GroupName = "LoginName or Email")]
        public string Email { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("LoginName", LoginName)
                          .AddRawPropertyValue("Email", Email)
                          .ToString();
        }

        #endregion
    }
}
