using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Webparts
{
    public class ContentByQueryWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentByQueryWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var typedDefinition = webPartModel.WithAssertAndCast<ContentByQueryWebPartDefinition>("model", value => value.RequireNotNull());
            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(this.ProcessCommonWebpartProperties(BuiltInWebPartTemplates.ContentByQueryWebPart, webPartModel));

            // xslt links
            if (!string.IsNullOrEmpty(typedDefinition.MainXslLink))
            {
                var linkValue = typedDefinition.MainXslLink;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original MainXslLink: [{0}]", linkValue);

                linkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = linkValue,
                    Context = listItemModelHost.HostClientContext
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced MainXslLink: [{0}]", linkValue);

                wpXml.SetOrUpdateProperty("MainXslLink", linkValue);
            }

            if (!string.IsNullOrEmpty(typedDefinition.ItemXslLink))
            {
                var linkValue = typedDefinition.ItemXslLink;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original ItemXslLink: [{0}]", linkValue);

                linkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = linkValue,
                    Context = listItemModelHost.HostClientContext
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced ItemXslLink: [{0}]", linkValue);

                wpXml.SetOrUpdateProperty("ItemXslLink", linkValue);
            }

            if (!string.IsNullOrEmpty(typedDefinition.HeaderXslLink))
            {
                var linkValue = typedDefinition.HeaderXslLink;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original HeaderXslLink: [{0}]", linkValue);

                linkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = linkValue,
                    Context = listItemModelHost.HostClientContext
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced HeaderXslLink: [{0}]", linkValue);

                wpXml.SetOrUpdateProperty("HeaderXslLink", linkValue);
            }

            // styles
            if (!string.IsNullOrEmpty(typedDefinition.ItemStyle))
                wpXml.SetItemStyle(typedDefinition.ItemStyle);

            if (!string.IsNullOrEmpty(typedDefinition.GroupStyle))
                wpXml.SetGroupStyle(typedDefinition.GroupStyle);


            // cache settings
            if (typedDefinition.UseCache.HasValue)
                wpXml.SetUseCache(typedDefinition.UseCache.ToString());

            if (typedDefinition.CacheXslStorage.HasValue)
                wpXml.SetOrUpdateProperty("CacheXslStorage", typedDefinition.CacheXslStorage.ToString());

            if (typedDefinition.CacheXslTimeOut.HasValue)
                wpXml.SetOrUpdateProperty("CacheXslTimeOut", typedDefinition.CacheXslTimeOut.ToString());

            // item limit
            if (typedDefinition.ItemLimit.HasValue)
                wpXml.SetOrUpdateProperty("ItemLimit", typedDefinition.ItemLimit.ToString());

            // mappings
            if (!string.IsNullOrEmpty(typedDefinition.DataMappings))
                wpXml.SetOrUpdateProperty("DataMappings", typedDefinition.DataMappings);

            if (!string.IsNullOrEmpty(typedDefinition.DataMappingViewFields))
                wpXml.SetOrUpdateProperty("DataMappingViewFields", typedDefinition.DataMappingViewFields);

            // misc
            if (typedDefinition.ShowUntargetedItems.HasValue)
                wpXml.SetOrUpdateProperty("ShowUntargetedItems", typedDefinition.ShowUntargetedItems.ToString());

            if (typedDefinition.PlayMediaInBrowser.HasValue)
                wpXml.SetOrUpdateProperty("PlayMediaInBrowser", typedDefinition.PlayMediaInBrowser.ToString());

            // FilterTypeXXX
            if (!string.IsNullOrEmpty(typedDefinition.FilterType1))
                wpXml.SetOrUpdateProperty("FilterType1", typedDefinition.FilterType1);

            if (!string.IsNullOrEmpty(typedDefinition.FilterType2))
                wpXml.SetOrUpdateProperty("FilterType2", typedDefinition.FilterType2);

            if (!string.IsNullOrEmpty(typedDefinition.FilterType3))
                wpXml.SetOrUpdateProperty("FilterType3", typedDefinition.FilterType3);

            // FilterFieldXXX
            if (!string.IsNullOrEmpty(typedDefinition.FilterField1))
                wpXml.SetOrUpdateProperty("FilterField1", typedDefinition.FilterField1);

            if (!string.IsNullOrEmpty(typedDefinition.FilterField2))
                wpXml.SetOrUpdateProperty("FilterField2", typedDefinition.FilterField2);

            if (!string.IsNullOrEmpty(typedDefinition.FilterField3))
                wpXml.SetOrUpdateProperty("FilterField3", typedDefinition.FilterField3);

            // FilterXXXIsCustomValue
            if (typedDefinition.Filter1IsCustomValue.HasValue)
                wpXml.SetOrUpdateProperty("Filter1IsCustomValue", typedDefinition.Filter1IsCustomValue.ToString());

            if (typedDefinition.Filter2IsCustomValue.HasValue)
                wpXml.SetOrUpdateProperty("Filter2IsCustomValue", typedDefinition.Filter2IsCustomValue.ToString());

            if (typedDefinition.Filter3IsCustomValue.HasValue)
                wpXml.SetOrUpdateProperty("Filter3IsCustomValue", typedDefinition.Filter3IsCustomValue.ToString());

            // FilterValueXXX
            if (!string.IsNullOrEmpty(typedDefinition.FilterValue1))
                wpXml.SetOrUpdateProperty("FilterValue1", typedDefinition.FilterValue1);

            if (!string.IsNullOrEmpty(typedDefinition.FilterValue2))
                wpXml.SetOrUpdateProperty("FilterValue2", typedDefinition.FilterValue2);

            if (!string.IsNullOrEmpty(typedDefinition.FilterValue3))
                wpXml.SetOrUpdateProperty("FilterValue3", typedDefinition.FilterValue3);

            var filterChainingOperatorType = "Microsoft.SharePoint.Publishing.WebControls.ContentByQueryWebPart+FilterChainingOperator, Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";

            if (!string.IsNullOrEmpty(typedDefinition.Filter1ChainingOperator))
                wpXml.SetTypedProperty("Filter1ChainingOperator", typedDefinition.Filter1ChainingOperator, filterChainingOperatorType);

            if (!string.IsNullOrEmpty(typedDefinition.Filter2ChainingOperator))
                wpXml.SetTypedProperty("Filter2ChainingOperator", typedDefinition.Filter2ChainingOperator, filterChainingOperatorType);


            // sorting
            if (!string.IsNullOrEmpty(typedDefinition.SortBy))
                wpXml.SetOrUpdateProperty("SortBy", typedDefinition.SortBy);

            if (!string.IsNullOrEmpty(typedDefinition.SortByDirection))
                wpXml.SetTypedProperty("SortByDirection", typedDefinition.SortByDirection, "Microsoft.SharePoint.Publishing.WebControls.ContentByQueryWebPart+SortDirection, Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");

            if (!string.IsNullOrEmpty(typedDefinition.SortByFieldType))
                wpXml.SetOrUpdateProperty("SortByFieldType", typedDefinition.SortByFieldType);

            if (!string.IsNullOrEmpty(typedDefinition.GroupByDirection))
                wpXml.SetTypedProperty("GroupByDirection", typedDefinition.GroupByDirection, "Microsoft.SharePoint.Publishing.WebControls.ContentByQueryWebPart+SortDirection, Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");

            var filterOperatorType = "Microsoft.SharePoint.Publishing.WebControls.ContentByQueryWebPart+FilterFieldQueryOperator, Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";


            // FilterOperatorXXX
            if (!string.IsNullOrEmpty(typedDefinition.FilterOperator1))
                wpXml.SetTypedProperty("FilterOperator1", typedDefinition.FilterOperator1, filterOperatorType);

            if (!string.IsNullOrEmpty(typedDefinition.FilterOperator2))
                wpXml.SetTypedProperty("FilterOperator2", typedDefinition.FilterOperator2, filterOperatorType);

            if (!string.IsNullOrEmpty(typedDefinition.FilterOperator3))
                wpXml.SetTypedProperty("FilterOperator3", typedDefinition.FilterOperator3, filterOperatorType);

            // FilterDisplayValueXXX

            if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue1))
                wpXml.SetOrUpdateProperty("FilterDisplayValue1", typedDefinition.FilterDisplayValue1);

            if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue2))
                wpXml.SetOrUpdateProperty("FilterDisplayValue2", typedDefinition.FilterDisplayValue2);

            if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue3))
                wpXml.SetOrUpdateProperty("FilterDisplayValue3", typedDefinition.FilterDisplayValue3);


            // bindings
            if (typedDefinition.ServerTemplate.HasValue)
                wpXml.SetOrUpdateProperty("ServerTemplate", typedDefinition.ServerTemplate.ToString());

            if (!string.IsNullOrEmpty(typedDefinition.ContentTypeName))
                wpXml.SetOrUpdateProperty("ContentTypeName", typedDefinition.ContentTypeName);

            if (!string.IsNullOrEmpty(typedDefinition.ContentTypeBeginsWithId))
                wpXml.SetOrUpdateProperty("ContentTypeBeginsWithId", typedDefinition.ContentTypeBeginsWithId);

            if (typedDefinition.ListId.HasGuidValue())
                wpXml.SetTypedProperty("ListId", typedDefinition.ListId.Value.ToString("D"), "Microsoft.SharePoint.Publishing.WebControls.ContentByQueryWebPart+FilterChainingOperator, Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");

            if (typedDefinition.ListGuid.HasGuidValue())
                wpXml.SetOrUpdateProperty("ListGuid", typedDefinition.ListGuid.Value.ToString("D"));

            if (!string.IsNullOrEmpty(typedDefinition.ListName))
                wpXml.SetOrUpdateProperty("ListName", typedDefinition.ListName);

            if (!string.IsNullOrEmpty(typedDefinition.WebUrl))
                wpXml.SetOrUpdateProperty("WebUrl", typedDefinition.WebUrl);

            // overrides
            if (!string.IsNullOrEmpty(typedDefinition.ListsOverride))
                wpXml.SetOrUpdateProperty("ListsOverride", typedDefinition.ListsOverride);

            if (!string.IsNullOrEmpty(typedDefinition.ViewFieldsOverride))
                wpXml.SetOrUpdateProperty("ViewFieldsOverride", typedDefinition.ViewFieldsOverride);

            if (!string.IsNullOrEmpty(typedDefinition.QueryOverride))
                wpXml.SetOrUpdateProperty("QueryOverride", typedDefinition.QueryOverride);

            if (!string.IsNullOrEmpty(typedDefinition.CommonViewFields))
                wpXml.SetOrUpdateProperty("CommonViewFields", typedDefinition.CommonViewFields);

            if (typedDefinition.FilterByAudience.HasValue)
                wpXml.SetOrUpdateProperty("FilterByAudience", typedDefinition.FilterByAudience.ToString());


            return wpXml.ToString();
        }

        #endregion
    }
}
