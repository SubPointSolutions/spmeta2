using System;
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

    public class ContentByQueryWebPartDefinition : WebPartDefinition
    {
        #region properties

        [DataMember]
        public string DataMappings { get; set; }

        [DataMember]
        public string DataMappingViewFields { get; set; }

        [DataMember]
        public bool? PlayMediaInBrowser { get; set; }

        [DataMember]
        public bool? UseCopyUtil { get; set; }

        [DataMember]
        public int? ItemLimit { get; set; }

        [DataMember]
        public int? ServerTemplate { get; set; }

        [DataMember]
        public string WebUrl { get; set; }

        [DataMember]
        public string ContentTypeBeginsWithId { get; set; }

        [DataMember]
        public Guid? ListGuid { get; set; }

        [DataMember]
        public Guid? ListUrl { get; set; }

        [DataMember]
        public Guid? ListName { get; set; }

        [DataMember]
        public string ItemStyle { get; set; }

        [DataMember]
        public string GroupStyle { get; set; }

        [DataMember]
        public bool? ShowUntargetedItems { get; set; }

        [DataMember]
        public string MainXslLink { get; set; }

        [DataMember]
        public string ItemXslLink { get; set; }

        [DataMember]
        public string FilterValue1 { get; set; }

        [DataMember]
        public string FilterValue2 { get; set; }

        [DataMember]
        public string FilterValue3 { get; set; }

        [DataMember]
        public string FilterDisplayValue1 { get; set; }

        [DataMember]
        public string FilterDisplayValue2 { get; set; }

        [DataMember]
        public string FilterDisplayValue3 { get; set; }

        [DataMember]
        public string FilterType1 { get; set; }

        [DataMember]
        public string FilterType2 { get; set; }

        [DataMember]
        public string FilterType3 { get; set; }

        [DataMember]
        public string FilterOperator1 { get; set; }

        [DataMember]
        public string FilterOperator2 { get; set; }

        [DataMember]
        public string FilterOperator3 { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ContentByQueryWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.DataMappings)
                          .AddPropertyValue(p => p.DataMappingViewFields)

                          .AddPropertyValue(p => p.ItemLimit)

                          .AddPropertyValue(p => p.WebUrl)
                          .AddPropertyValue(p => p.ListGuid)

                          .AddPropertyValue(p => p.ItemStyle)
                          .AddPropertyValue(p => p.GroupStyle)

                          .AddPropertyValue(p => p.MainXslLink)
                          .AddPropertyValue(p => p.ItemXslLink)

                          .ToString();
        }

        #endregion
    }
}
