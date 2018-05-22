using System;
using System.Reflection;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'Content by Query' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ExpectWebpartType(WebPartType = "Microsoft.Office.Server.Search.WebControls.ContentBySearchWebPart, Microsoft.Office.Server.Search, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class ContentBySearchWebPartDefinition : WebPartDefinition
    {
        #region properties

        [DataMember]
        [ExpectValidation]
        public string RenderTemplateId { get; set; }

        [DataMember]
        [ExpectValidation]
        public string ItemTemplateId { get; set; }

        [DataMember]
        [ExpectValidation]
        public string GroupTemplateId { get; set; }

        [DataMember]
        [ExpectValidation]
        public string DataProviderJSON { get; set; }

        [DataMember]
        [ExpectValidation]
        public int? NumberOfItems { get; set; }

        [DataMember]
        [ExpectValidation]
        public int? ResultsPerPage { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? OverwriteResultPath { get; set; }

        [DataMember]
        [ExpectValidation]
        public string PropertyMappings { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShouldHideControlWhenEmpty { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? LogAnalyticsViewEvent { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? AddSEOPropertiesFromSearch { get; set; }

        [DataMember]
        [ExpectValidation]
        public int? StartingItemIndex { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())

                          .ToString();
        }

        #endregion
    }
}
