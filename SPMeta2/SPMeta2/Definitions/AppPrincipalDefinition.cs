using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPAppPrincipal", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.AppPrincipal", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]
    public class AppPrincipalDefinition : DefinitionBase
    {
        #region properties

        public string AppId { get; set; }
        public string AppSecret { get; set; }

        public string Title { get; set; }

        public string AppDomain { get; set; }
        public string RedirectURI { get; set; }

        #endregion
    }
}
