using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy SharePoint wiki page.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectAddHostExtensionMethod]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ParentHostCapability(typeof(ListDefinition))]
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
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("Group", Group)
                          .ToString();
        }

        #endregion
    }
}
