using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPAppPrincipal", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.AppPrincipal", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]

    [ExpectManyInstances]

    [ParentHostCapability(typeof(WebDefinition))]
    public class AppPrincipalDefinition : DefinitionBase
    {
        #region properties

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        [IdentityKey]
        public string AppId { get; set; }

        [DataMember]
        public string AppSecret { get; set; }

        [DataMember]
        public string AppDomain { get; set; }

        [DataMember]
        public string RedirectURI { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Title", Title)

                          .AddRawPropertyValue("AppId", AppId)
                          .AddRawPropertyValue("AppSecret", AppSecret)

                          .AddRawPropertyValue("AppDomain", AppDomain)
                          .AddRawPropertyValue("RedirectURI", RedirectURI)
                          .ToString();
        }

        #endregion
    }
}
