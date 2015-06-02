using System;
using Microsoft.SharePoint.Portal.WebControls;
using Microsoft.SharePoint.Publishing.WebControls;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Webparts
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

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<ContentByQueryWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(ContentByQueryWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition definition)
        {
            base.ProcessWebpartProperties(webpartInstance, definition);

            var typedWebpart = webpartInstance.WithAssertAndCast<ContentByQueryWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedDefinition = definition.WithAssertAndCast<ContentByQueryWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            // xslt links
            if (!string.IsNullOrEmpty(typedDefinition.MainXslLink))
            {
                var linkValue = typedDefinition.MainXslLink;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original MainXslLink: [{0}]", linkValue);

                linkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = linkValue,
                    Context = CurrentHost.PageListItem.Web
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced MainXslLink: [{0}]", linkValue);

                typedWebpart.MainXslLink = linkValue;
            }

            if (!string.IsNullOrEmpty(typedDefinition.ItemXslLink))
            {
                var linkValue = typedDefinition.ItemXslLink;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original ItemXslLink: [{0}]", linkValue);

                linkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = linkValue,
                    Context = CurrentHost.PageListItem.Web
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced ItemXslLink: [{0}]", linkValue);

                typedWebpart.ItemXslLink = linkValue;
            }

            if (!string.IsNullOrEmpty(typedDefinition.HeaderXslLink))
            {
                var linkValue = typedDefinition.HeaderXslLink;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original HeaderXslLink: [{0}]", linkValue);

                linkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = linkValue,
                    Context = CurrentHost.PageListItem.Web
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced HeaderXslLink: [{0}]", linkValue);

                typedWebpart.HeaderXslLink = linkValue;
            }

            // styles
            if (!string.IsNullOrEmpty(typedDefinition.ItemStyle))
                typedWebpart.ItemStyle = typedDefinition.ItemStyle;

            if (!string.IsNullOrEmpty(typedDefinition.GroupStyle))
                typedWebpart.GroupStyle = typedDefinition.GroupStyle;


            // cache settings
            if (typedDefinition.UseCache.HasValue)
                typedWebpart.UseCache = typedDefinition.UseCache.Value;

            if (typedDefinition.CacheXslStorage.HasValue)
                typedWebpart.CacheXslStorage = typedDefinition.CacheXslStorage.Value;

            if (typedDefinition.CacheXslTimeOut.HasValue)
                typedWebpart.CacheXslTimeOut = typedDefinition.CacheXslTimeOut.Value;

            // item limit
            if (typedDefinition.ItemLimit.HasValue)
                typedWebpart.ItemLimit = typedDefinition.ItemLimit.Value;

            // mappings
            if (!string.IsNullOrEmpty(typedDefinition.DataMappings))
                typedWebpart.DataMappings = typedDefinition.DataMappings;

            if (!string.IsNullOrEmpty(typedDefinition.DataMappingViewFields))
                typedWebpart.DataMappingViewFields = typedDefinition.DataMappingViewFields;

            // misc
            if (typedDefinition.ShowUntargetedItems.HasValue)
                typedWebpart.ShowUntargetedItems = typedDefinition.ShowUntargetedItems.Value;

            if (typedDefinition.PlayMediaInBrowser.HasValue)
                typedWebpart.PlayMediaInBrowser = typedDefinition.PlayMediaInBrowser.Value;

            // FilterTypeXXX
            if (!string.IsNullOrEmpty(typedDefinition.FilterType1))
                typedWebpart.FilterType1 = typedDefinition.FilterType1;

            if (!string.IsNullOrEmpty(typedDefinition.FilterType2))
                typedWebpart.FilterType2 = typedDefinition.FilterType2;

            if (!string.IsNullOrEmpty(typedDefinition.FilterType3))
                typedWebpart.FilterType3 = typedDefinition.FilterType3;

            // FilterFieldXXX
            if (!string.IsNullOrEmpty(typedDefinition.FilterField1))
                typedWebpart.FilterField1 = typedDefinition.FilterField1;

            if (!string.IsNullOrEmpty(typedDefinition.FilterField2))
                typedWebpart.FilterField2 = typedDefinition.FilterField2;

            if (!string.IsNullOrEmpty(typedDefinition.FilterField3))
                typedWebpart.FilterField3 = typedDefinition.FilterField3;

            // FilterXXXIsCustomValue
            if (typedDefinition.Filter1IsCustomValue.HasValue)
                typedWebpart.Filter1IsCustomValue = typedDefinition.Filter1IsCustomValue.Value;

            if (typedDefinition.Filter2IsCustomValue.HasValue)
                typedWebpart.Filter2IsCustomValue = typedDefinition.Filter2IsCustomValue.Value;

            if (typedDefinition.Filter3IsCustomValue.HasValue)
                typedWebpart.Filter3IsCustomValue = typedDefinition.Filter3IsCustomValue.Value;

            // FilterValueXXX
            if (!string.IsNullOrEmpty(typedDefinition.FilterValue1))
                typedWebpart.FilterValue1 = typedDefinition.FilterValue1;

            if (!string.IsNullOrEmpty(typedDefinition.FilterValue2))
                typedWebpart.FilterValue2 = typedDefinition.FilterValue2;

            if (!string.IsNullOrEmpty(typedDefinition.FilterValue3))
                typedWebpart.FilterValue3 = typedDefinition.FilterValue3;


            if (!string.IsNullOrEmpty(typedDefinition.Filter1ChainingOperator))
            {
                typedWebpart.Filter1ChainingOperator = (ContentByQueryWebPart.FilterChainingOperator)
                   Enum.Parse(typeof(ContentByQueryWebPart.FilterChainingOperator), typedDefinition.Filter1ChainingOperator);
            }

            if (!string.IsNullOrEmpty(typedDefinition.Filter2ChainingOperator))
            {
                typedWebpart.Filter2ChainingOperator = (ContentByQueryWebPart.FilterChainingOperator)
                   Enum.Parse(typeof(ContentByQueryWebPart.FilterChainingOperator), typedDefinition.Filter2ChainingOperator);
            }


            // sorting
            if (!string.IsNullOrEmpty(typedDefinition.SortBy))
                typedWebpart.SortBy = typedDefinition.SortBy;

            if (!string.IsNullOrEmpty(typedDefinition.SortByDirection))
                typedWebpart.SortByDirection = (ContentByQueryWebPart.SortDirection)
                    Enum.Parse(typeof(ContentByQueryWebPart.SortDirection), typedDefinition.SortByDirection);

            if (!string.IsNullOrEmpty(typedDefinition.SortByFieldType))
                typedWebpart.SortByFieldType = typedDefinition.SortByFieldType;

            if (!string.IsNullOrEmpty(typedDefinition.GroupByDirection))
            {
                typedWebpart.GroupByDirection = (ContentByQueryWebPart.SortDirection)
                    Enum.Parse(typeof(ContentByQueryWebPart.SortDirection), typedDefinition.GroupByDirection);
            }


            // FilterOperatorXXX
            if (!string.IsNullOrEmpty(typedDefinition.FilterOperator1))
            {
                typedWebpart.FilterOperator1 = (ContentByQueryWebPart.FilterFieldQueryOperator)
                    Enum.Parse(typeof(ContentByQueryWebPart.FilterFieldQueryOperator), typedDefinition.FilterOperator1);
            }

            if (!string.IsNullOrEmpty(typedDefinition.FilterOperator2))
            {
                typedWebpart.FilterOperator2 = (ContentByQueryWebPart.FilterFieldQueryOperator)
                    Enum.Parse(typeof(ContentByQueryWebPart.FilterFieldQueryOperator), typedDefinition.FilterOperator2);
            }

            if (!string.IsNullOrEmpty(typedDefinition.FilterOperator3))
            {
                typedWebpart.FilterOperator3 = (ContentByQueryWebPart.FilterFieldQueryOperator)
                    Enum.Parse(typeof(ContentByQueryWebPart.FilterFieldQueryOperator), typedDefinition.FilterOperator3);
            }

            // FilterDisplayValueXXX

            if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue1))
                typedWebpart.FilterDisplayValue1 = typedDefinition.FilterDisplayValue1;

            if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue2))
                typedWebpart.FilterDisplayValue2 = typedDefinition.FilterDisplayValue2;

            if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue3))
                typedWebpart.FilterDisplayValue3 = typedDefinition.FilterDisplayValue3;


            // bindings
            if (typedDefinition.ServerTemplate.HasValue)
                typedWebpart.ServerTemplate = typedDefinition.ServerTemplate.ToString();

            if (!string.IsNullOrEmpty(typedDefinition.ContentTypeName))
                typedWebpart.ContentTypeName = typedDefinition.ContentTypeName;

            if (!string.IsNullOrEmpty(typedDefinition.ContentTypeBeginsWithId))
                typedWebpart.ContentTypeBeginsWithId = typedDefinition.ContentTypeBeginsWithId;

            if (typedDefinition.ListId.HasGuidValue())
                typedWebpart.ListId = typedDefinition.ListId.Value;

            if (typedDefinition.ListGuid.HasGuidValue())
                typedWebpart.ListGuid = typedDefinition.ListGuid.Value.ToString("D");

            if (!string.IsNullOrEmpty(typedDefinition.ListName))
                typedWebpart.ListName = typedDefinition.ListName;

            if (!string.IsNullOrEmpty(typedDefinition.WebUrl))
                typedWebpart.WebUrl = typedDefinition.WebUrl;

            // overrides
            if (!string.IsNullOrEmpty(typedDefinition.ListsOverride))
                typedWebpart.ListsOverride = typedDefinition.ListsOverride;

            if (!string.IsNullOrEmpty(typedDefinition.ViewFieldsOverride))
                typedWebpart.ViewFieldsOverride = typedDefinition.ViewFieldsOverride;

            if (!string.IsNullOrEmpty(typedDefinition.QueryOverride))
                typedWebpart.QueryOverride = typedDefinition.QueryOverride;

            if (!string.IsNullOrEmpty(typedDefinition.CommonViewFields))
                typedWebpart.CommonViewFields = typedDefinition.CommonViewFields;

            if (typedDefinition.FilterByAudience.HasValue)
                typedWebpart.FilterByAudience = typedDefinition.FilterByAudience.Value;
        }

        #endregion
    }
}
