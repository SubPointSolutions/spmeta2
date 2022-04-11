using System;
using System.Reflection;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

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

    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.Publishing.WebControls.ContentByQueryWebPart, Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class ContentByQueryWebPartDefinition : WebPartDefinition
    {
        #region constructor

        public ContentByQueryWebPartDefinition()
        {
            ItemStyle = "Default";
            GroupStyle = "DefaultHeader";
        }

        #endregion

        #region properties

        [DataMember]
        [ExpectValidation]
        public string SortBy { get; set; }

        [DataMember]
        [ExpectValidation]
        public string SortByDirection { get; set; }

        [DataMember]
        [ExpectValidation]
        public string SortByFieldType { get; set; }

        [DataMember]
        [ExpectValidation]
        public string DataMappings { get; set; }

        [DataMember]
        [ExpectValidation]
        public string DataMappingViewFields { get; set; }

        [DataMember]
        [ExpectValidation]
        //[ExpectUpdate]
        public bool? PlayMediaInBrowser { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? UseCopyUtil { get; set; }

        [DataMember]
        [ExpectValidation]
        //[ExpectUpdateAsIntRange(MinValue = 10, MaxValue = 100)]
        public int? ItemLimit { get; set; }

        [DataMember]
        [ExpectValidation]
        public int? ServerTemplate { get; set; }

        [DataMember]
        [ExpectValidation]
        public string WebUrl { get; set; }

        [DataMember]
        [ExpectValidation]
        public Guid? WebId { get; set; }

        [DataMember]
        [ExpectValidation]
        public string ContentTypeBeginsWithId { get; set; }

        [DataMember]
        [ExpectValidation]
        public Guid? ListGuid { get; set; }

        [DataMember]
        [ExpectValidation]
        public string ListUrl { get; set; }

        [DataMember]
        [ExpectValidation]
        public string ListName { get; set; }

        [DataMember]
        [ExpectValidation]
        public string ItemStyle { get; set; }

        [DataMember]
        [ExpectValidation]
        public string GroupStyle { get; set; }

        [DataMember]
        [ExpectValidation]
        // [ExpectUpdate]
        public bool? ShowUntargetedItems { get; set; }

        [DataMember]
        [ExpectValidation]

        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string MainXslLink { get; set; }

        [DataMember]
        [ExpectValidation]

        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string ItemXslLink { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterValue1 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterValue2 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterValue3 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterDisplayValue1 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterDisplayValue2 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterDisplayValue3 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterType1 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterType2 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterType3 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterOperator1 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterOperator2 { get; set; }

        [DataMember]
        [ExpectValidation]
        public string FilterOperator3 { get; set; }



        [DataMember]

        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string HeaderXslLink { get; set; }

        [DataMember]
        public bool? UseCache { get; set; }

        [DataMember]
        public bool? CacheXslStorage { get; set; }

        [DataMember]
        public string FilterField1 { get; set; }

        [DataMember]
        public string FilterField2 { get; set; }

        [DataMember]
        public string FilterField3 { get; set; }

        [DataMember]
        public string ContentTypeName { get; set; }

        [DataMember]
        public Guid? ListId { get; set; }

        [DataMember]
        public string ListsOverride { get; set; }

        [DataMember]
        public string ViewFieldsOverride { get; set; }

        [DataMember]
        public bool? FilterByAudience { get; set; }

        [DataMember]
        public string CommonViewFields { get; set; }

        [DataMember]
        public string QueryOverride { get; set; }

        [DataMember]
        public string Filter1ChainingOperator { get; set; }

        [DataMember]
        public string Filter2ChainingOperator { get; set; }

        [DataMember]
        public int? CacheXslTimeOut { get; set; }

        [DataMember]
        public string GroupByDirection { get; set; }

        [DataMember]
        [ExpectValidation]
        public string GroupBy { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 1, MaxValue = 10)]
        public int? DisplayColumns { get; set; }

        [DataMember]
        public bool? Filter1IsCustomValue { get; set; }

        [DataMember]
        public bool? Filter2IsCustomValue { get; set; }

        [DataMember]
        public bool? Filter3IsCustomValue { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("DataMappings", DataMappings)
                          .AddRawPropertyValue("DataMappingViewFields", DataMappingViewFields)

                          .AddRawPropertyValue("ItemLimit", ItemLimit)

                          .AddRawPropertyValue("WebUrl", WebUrl)
                          .AddRawPropertyValue("ListGuid", ListGuid)
                          .AddRawPropertyValue("ListName", ListName)
                          .AddRawPropertyValue("ListUrl", ListUrl)

                          .AddRawPropertyValue("ItemStyle", ItemStyle)
                          .AddRawPropertyValue("GroupStyle", GroupStyle)

                          .AddRawPropertyValue("MainXslLink", MainXslLink)
                          .AddRawPropertyValue("ItemXslLink", ItemXslLink)

                          .ToString();
        }

        #endregion
    }
}
