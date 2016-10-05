using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

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
        [ExpectUpdate]
        [DataMember]
        public string LoginName { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string Email { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<UserDefinition>(this, base.ToString())

                          .ToString();
        }

        #endregion
    }
}
