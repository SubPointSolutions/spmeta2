using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint asp.net master page.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ListDefinition), typeof(RootWebDefinition))]

    [Serializable]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    public class WebPartGalleryFileDefinition : PageDefinitionBase
    {
        #region properties

        public string Description { get; set; }
        public string Group { get; set; }
        public string RecommendationSettings { get; set; }

        #endregion
    }
}
