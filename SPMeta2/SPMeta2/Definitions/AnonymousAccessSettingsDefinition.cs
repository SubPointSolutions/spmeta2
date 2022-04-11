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

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]


    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    //[ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]
    [SelfHostCapability]
    [SingletonIdentity]
    public class AnonymousAccessSettingsDefinition : DefinitionBase
    {
        public AnonymousAccessSettingsDefinition()
        {
            AnonymousPermMask64 = new Collection<string>();
        }

        #region properties

        /// <summary>
        ///Mapped to WebAnonymousState 
        /// - Disabled
        /// - Enabled
        /// - On
        /// </summary>
        [DataMember]
        [IdentityKey]
        public string AnonymousState { get; set; }

        [DataMember]
        [IdentityKey]
        public Collection<string> AnonymousPermMask64 { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("AnonymousState", AnonymousState)
                          .ToString();
        }

        #endregion
    }
}
