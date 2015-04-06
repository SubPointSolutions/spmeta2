using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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
    [DataContract]
    //[ExpectAddHostExtensionMethod]
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
        [DataMember]
        public string Description { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public string Group { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public List<string> RecommendationSettings { get; set; }

        #endregion

        #region methods

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<WebPartGalleryFileDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.Group)
                          .ToString();
        }

        #endregion
    }
}
