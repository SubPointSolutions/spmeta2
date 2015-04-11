using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPAppPrincipal", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.AppPrincipal", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]

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
            return new ToStringResult<AppPrincipalDefinition>(this)
                          .AddPropertyValue(p => p.Title)

                          .AddPropertyValue(p => p.AppId)
                          .AddPropertyValue(p => p.AppSecret)

                          .AddPropertyValue(p => p.AppDomain)
                          .AddPropertyValue(p => p.RedirectURI)
                          .ToString();
        }

        #endregion
    }
}
