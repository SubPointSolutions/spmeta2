using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Webparts
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

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var definition = webPartModel.WithAssertAndCast<ContentBySearchWebPartDefinition>("model", value => value.RequireNotNull());
            var xml = WebpartXmlExtensions.LoadWebpartXmlDocument(BuiltInWebPartTemplates.ContentByQueryWebPart);

            // JSON
            if (!string.IsNullOrEmpty(definition.DataProviderJSON))
                xml.SetDataProviderJSON(definition.DataProviderJSON);

            // templates
            if (!string.IsNullOrEmpty(definition.GroupTemplateId))
                xml.SetGroupTemplateId(definition.GroupTemplateId);

            if (!string.IsNullOrEmpty(definition.ItemTemplateId))
                xml.SetItemTemplateId(definition.ItemTemplateId);

            if (!string.IsNullOrEmpty(definition.RenderTemplateId))
                xml.SetRenderTemplateId(definition.RenderTemplateId);

            // item counts
            if (definition.NumberOfItems.HasValue)
                xml.SetNumberOfItems(definition.NumberOfItems.Value);

            if (definition.ResultsPerPage.HasValue)
                xml.SetResultsPerPage(definition.ResultsPerPage.Value);

            // misc
            if (!string.IsNullOrEmpty(definition.PropertyMappings))
                xml.SetPropertyMappings(definition.PropertyMappings);

            if (definition.OverwriteResultPath.HasValue)
                xml.SetOverwriteResultPath(definition.OverwriteResultPath.Value);

            if (definition.ShouldHideControlWhenEmpty.HasValue)
                xml.SetShouldHideControlWhenEmpty(definition.ShouldHideControlWhenEmpty.Value);

            if (definition.LogAnalyticsViewEvent.HasValue)
                xml.SetLogAnalyticsViewEvent(definition.LogAnalyticsViewEvent.Value);

            if (definition.AddSEOPropertiesFromSearch.HasValue)
                xml.SetAddSEOPropertiesFromSearch(definition.AddSEOPropertiesFromSearch.Value);

            if (definition.StartingItemIndex.HasValue)
                xml.SetStartingItemIndex(definition.StartingItemIndex.Value);

            return xml.ToString();
        }

        #endregion
    }
}
