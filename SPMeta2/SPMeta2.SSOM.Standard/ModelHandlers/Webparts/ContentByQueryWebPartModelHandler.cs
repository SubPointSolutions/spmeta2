using System;
using Microsoft.SharePoint.Portal.WebControls;
using Microsoft.SharePoint.Publishing.WebControls;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
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
            var typedModel = definition.WithAssertAndCast<ContentByQueryWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            // lists
            if (!string.IsNullOrEmpty(typedModel.ListUrl))
                typedWebpart.ListUrl = typedModel.ListUrl;

            if (!string.IsNullOrEmpty(typedModel.ListName))
                typedWebpart.ListName = typedModel.ListName;

            if (typedModel.ListGuid.HasGuidValue())
                typedWebpart.ListGuid = typedModel.ListGuid.Value.ToString();

            // server template
            if (typedModel.ServerTemplate.HasValue)
                typedWebpart.ServerTemplate = typedModel.ServerTemplate.ToString();

            if (!string.IsNullOrEmpty(typedModel.ContentTypeBeginsWithId))
                typedWebpart.ContentTypeBeginsWithId = typedModel.ContentTypeBeginsWithId;

            // filter value 1-2-3
            if (!string.IsNullOrEmpty(typedModel.FilterValue1))
                typedWebpart.FilterValue1 = typedModel.FilterValue1;

            if (!string.IsNullOrEmpty(typedModel.FilterValue2))
                typedWebpart.FilterValue2 = typedModel.FilterValue2;

            if (!string.IsNullOrEmpty(typedModel.FilterValue3))
                typedWebpart.FilterValue3 = typedModel.FilterValue3;

            // filter display value 1-2-3

            if (!string.IsNullOrEmpty(typedModel.FilterDisplayValue1))
                typedWebpart.FilterDisplayValue1 = typedModel.FilterDisplayValue1;

            if (!string.IsNullOrEmpty(typedModel.FilterDisplayValue2))
                typedWebpart.FilterDisplayValue2 = typedModel.FilterDisplayValue2;

            if (!string.IsNullOrEmpty(typedModel.FilterDisplayValue3))
                typedWebpart.FilterDisplayValue3 = typedModel.FilterDisplayValue3;

            // filter type 1-2-3
            if (!string.IsNullOrEmpty(typedModel.FilterType1))
                typedWebpart.FilterType1 = typedModel.FilterType1;

            if (!string.IsNullOrEmpty(typedModel.FilterType2))
                typedWebpart.FilterType2 = typedModel.FilterType2;

            if (!string.IsNullOrEmpty(typedModel.FilterType3))
                typedWebpart.FilterType3 = typedModel.FilterType3;

            // filter operator 1-2-3

            if (!string.IsNullOrEmpty(typedModel.FilterOperator1))
            {
                typedWebpart.FilterOperator1 = (ContentByQueryWebPart.FilterFieldQueryOperator)
                    Enum.Parse(typeof(ContentByQueryWebPart.FilterFieldQueryOperator), typedModel.FilterOperator1);
            }

            if (!string.IsNullOrEmpty(typedModel.FilterOperator2))
            {
                typedWebpart.FilterOperator2 = (ContentByQueryWebPart.FilterFieldQueryOperator)
                    Enum.Parse(typeof(ContentByQueryWebPart.FilterFieldQueryOperator), typedModel.FilterOperator2);
            }

            if (!string.IsNullOrEmpty(typedModel.FilterOperator3))
            {
                typedWebpart.FilterOperator3 = (ContentByQueryWebPart.FilterFieldQueryOperator)
                    Enum.Parse(typeof(ContentByQueryWebPart.FilterFieldQueryOperator), typedModel.FilterOperator3);
            }

            // sorting
            if (!string.IsNullOrEmpty(typedModel.SortBy))
                typedWebpart.SortBy = typedModel.SortBy;

            if (!string.IsNullOrEmpty(typedModel.SortByDirection))
                typedWebpart.SortByDirection = (ContentByQueryWebPart.SortDirection)
                    Enum.Parse(typeof(ContentByQueryWebPart.SortDirection), typedModel.SortByDirection);

            if (!string.IsNullOrEmpty(typedModel.SortByFieldType))
                typedWebpart.SortByFieldType = typedModel.SortByFieldType;

            // data mappings
            if (!string.IsNullOrEmpty(typedModel.DataMappings))
                typedWebpart.DataMappings = typedModel.DataMappings;

            if (!string.IsNullOrEmpty(typedModel.DataMappingViewFields))
                typedWebpart.DataMappingViewFields = typedModel.DataMappingViewFields;

            //  xslt styles

            if (!string.IsNullOrEmpty(typedModel.ItemStyle))
                typedWebpart.ItemStyle = typedModel.ItemStyle;

            if (!string.IsNullOrEmpty(typedModel.GroupStyle))
                typedWebpart.GroupStyle = typedModel.GroupStyle;

            // xslt files

            // TODO, add token support later
            if (!string.IsNullOrEmpty(typedModel.ItemXslLink))
                typedWebpart.ItemXslLink = typedModel.ItemXslLink;

            if (!string.IsNullOrEmpty(typedModel.MainXslLink))
                typedWebpart.MainXslLink = typedModel.MainXslLink;

            // misc
            if (typedModel.UseCopyUtil.HasValue)
                typedWebpart.UseCopyUtil = typedModel.UseCopyUtil.Value;

            if (typedModel.ItemLimit.HasValue)
                typedWebpart.ItemLimit = typedModel.ItemLimit.Value;

            if (typedModel.PlayMediaInBrowser.HasValue)
                typedWebpart.PlayMediaInBrowser = typedModel.PlayMediaInBrowser.Value;

            if (typedModel.ShowUntargetedItems.HasValue)
                typedWebpart.ShowUntargetedItems = typedModel.ShowUntargetedItems.Value;
        }

        #endregion
    }
}
