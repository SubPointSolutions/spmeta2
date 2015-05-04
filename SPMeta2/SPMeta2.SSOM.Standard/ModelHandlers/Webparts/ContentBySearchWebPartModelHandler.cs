using System;
using Microsoft.Office.Server.Search.WebControls;
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
    public class ContentBySearchWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentBySearchWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<ContentBySearchWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(ContentBySearchWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition definition)
        {
            base.ProcessWebpartProperties(webpartInstance, definition);

            var typedWebpart = webpartInstance.WithAssertAndCast<ContentBySearchWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedModel = definition.WithAssertAndCast<ContentBySearchWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            // templates
            if (!string.IsNullOrEmpty(typedModel.GroupTemplateId))
                typedWebpart.GroupTemplateId = typedModel.GroupTemplateId;

            if (!string.IsNullOrEmpty(typedModel.ItemTemplateId))
                typedWebpart.ItemTemplateId = typedModel.ItemTemplateId;

            if (!string.IsNullOrEmpty(typedModel.RenderTemplateId))
                typedWebpart.RenderTemplateId = typedModel.RenderTemplateId;

            if (!string.IsNullOrEmpty(typedModel.DataProviderJSON))
                typedWebpart.DataProviderJSON = typedModel.DataProviderJSON;

            if (!string.IsNullOrEmpty(typedModel.PropertyMappings))
                typedWebpart.PropertyMappings = typedModel.PropertyMappings;

            if (typedModel.OverwriteResultPath.HasValue)
                typedWebpart.OverwriteResultPath = typedModel.OverwriteResultPath.Value;

            if (typedModel.ShouldHideControlWhenEmpty.HasValue)
                typedWebpart.ShouldHideControlWhenEmpty = typedModel.ShouldHideControlWhenEmpty.Value;

            if (typedModel.LogAnalyticsViewEvent.HasValue)
                typedWebpart.LogAnalyticsViewEvent = typedModel.LogAnalyticsViewEvent.Value;

            if (typedModel.AddSEOPropertiesFromSearch.HasValue)
                typedWebpart.AddSEOPropertiesFromSearch = typedModel.AddSEOPropertiesFromSearch.Value;

            if (typedModel.StartingItemIndex.HasValue)
                typedWebpart.StartingItemIndex = typedModel.StartingItemIndex.Value;

            // misc
            if (typedModel.NumberOfItems.HasValue)
                typedWebpart.NumberOfItems = typedModel.NumberOfItems.Value;

            if (typedModel.ResultsPerPage.HasValue)
                typedWebpart.ResultsPerPage = typedModel.ResultsPerPage.Value;
        }

        #endregion
    }
}
