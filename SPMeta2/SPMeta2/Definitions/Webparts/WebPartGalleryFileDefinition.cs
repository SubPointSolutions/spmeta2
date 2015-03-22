using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy SharePoint wiki page.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [Serializable]
    [ExpectAddHostExtensionMethod]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    public class WebPartGalleryFileDefinition : ContentPageDefinitionBase
    {
        #region constructors

        public WebPartGalleryFileDefinition()
        {
            RecommendationSettings = new List<string>();
        }

        #endregion

        #region properties

        [ExpectUpdate]
        [ExpectValidation]
        public string Description { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public string Group { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public List<string> RecommendationSettings { get; set; }

        #endregion

        #region methods

        #endregion
    }
}
