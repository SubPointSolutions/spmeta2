using System;
using System.Web.Script.Serialization;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.Config;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class SearchSettingsModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SearchSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            //var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SearchSettingsDefinition>("model", value => value.RequireNotNull());

            if (modelHost is SiteModelHost)
            {
                DeployAtSiteLevel(modelHost, (modelHost as SiteModelHost).HostSite, definition);
            }
            else if (modelHost is WebModelHost)
            {
                DeployAtWebLevel(modelHost, (modelHost as WebModelHost).HostWeb, definition);
            }
        }

        protected virtual string GetSearchCenterUrlAtWebLevel(Web web)
        {
            return InternalGetSearchCenterUrl(web, true);
        }

        protected virtual string GetSearchCenterUrlAtSiteLevel(Web web)
        {
            return InternalGetSearchCenterUrl(web, false);
        }

        private string InternalGetSearchCenterUrl(Web web, bool isWebLevel)
        {
            var propertyBagName = "SRCH_ENH_FTR_URL_SITE";

            if (isWebLevel)
                propertyBagName = "SRCH_ENH_FTR_URL_WEB";

            if (!web.AllProperties.FieldValues.ContainsKey(propertyBagName))
                return string.Empty;

            return ConvertUtils.ToString(web.AllProperties[propertyBagName]);
        }

        protected virtual void SetSearchCenterUrlAtWebLevel(CSOMModelHostBase modelHost, Web web, string url)
        {
            InternalSetSearchCenterUrl(modelHost, web, url, true);
        }

        protected virtual void SetSearchCenterUrlAtSiteLevel(CSOMModelHostBase modelHost, Web web, string url)
        {
            InternalSetSearchCenterUrl(modelHost, web, url, false);
        }

        protected void InternalSetSearchCenterUrl(CSOMModelHostBase modelHost, Web web, string url, bool isWebLevel)
        {
            var propertyBagName = "SRCH_ENH_FTR_URL_SITE";

            if (isWebLevel)
                propertyBagName = "SRCH_ENH_FTR_URL_WEB";

            if (!string.IsNullOrEmpty(url))
            {
                url = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Context = modelHost,
                    Value = url
                }).Value;

                var props = web.AllProperties;
                props[propertyBagName] = url;
            }
        }

        private void DeployAtWebLevel(object modelHost, Web web, SearchSettingsDefinition definition)
        {
            var csomModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            var context = web.Context;

            context.Load(web);
            context.Load(web, w => w.AllProperties);

            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            var searchSettings = GetCurrentSearchConfigAtWebLevel(web);

            if (searchSettings != null)
            {
                if (definition.UseParentResultsPageUrl.HasValue)
                    searchSettings.Inherit = definition.UseParentResultsPageUrl.Value;

                if (!string.IsNullOrEmpty(definition.UseCustomResultsPageUrl))
                {
                    var url = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Context = csomModelHost,
                        Value = definition.UseCustomResultsPageUrl
                    }).Value;

                    searchSettings.ResultsPageAddress = url;
                }

                SetCurrentSearchConfigAtWebLevel(csomModelHost, web, searchSettings);
            }

            if (!string.IsNullOrEmpty(definition.SearchCenterUrl))
            {
                SetSearchCenterUrlAtWebLevel(csomModelHost, web, definition.SearchCenterUrl);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            web.Update();
            context.ExecuteQueryWithTrace();
        }

        private void DeployAtSiteLevel(object modelHost, Site site, SearchSettingsDefinition definition)
        {
            var csomModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            var web = site.RootWeb;

            var context = web.Context;

            context.Load(web);
            context.Load(web, w => w.AllProperties);

            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(definition.SearchCenterUrl))
            {
                SetSearchCenterUrlAtSiteLevel(csomModelHost, web, definition.SearchCenterUrl);
            }

            var searchSettings = GetCurrentSearchConfigAtSiteLevel(web);

            if (searchSettings != null)
            {
                if (definition.UseParentResultsPageUrl.HasValue)
                    searchSettings.Inherit = definition.UseParentResultsPageUrl.Value;

                if (!string.IsNullOrEmpty(definition.UseCustomResultsPageUrl))
                {
                    var url = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Context = csomModelHost,
                        Value = definition.UseCustomResultsPageUrl
                    }).Value;


                    searchSettings.ResultsPageAddress = url;
                }

                SetCurrentSearchConfigAtSiteLevel(csomModelHost, web, searchSettings);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            web.Update();
            context.ExecuteQueryWithTrace();
        }

        protected virtual void SetCurrentSearchConfigAtWebLevel(CSOMModelHostBase modelHost, Web web, SearchSettingsConfig searchSettings)
        {
            InternalSetCurrentSearchConfig(modelHost, web, searchSettings, true);
        }

        protected virtual void SetCurrentSearchConfigAtSiteLevel(CSOMModelHostBase modelHost, Web web, SearchSettingsConfig searchSettings)
        {
            InternalSetCurrentSearchConfig(modelHost, web, searchSettings, false);
        }

        private void InternalSetCurrentSearchConfig(
            CSOMModelHostBase modelHost,
            Web web,
            SearchSettingsConfig searchSettings, bool isWebLevel)
        {
            var propertyBagName = "SRCH_SB_SET_SITE";

            if (isWebLevel)
                propertyBagName = "SRCH_SB_SET_WEB";

            var serializer = new JavaScriptSerializer();

            var props = web.AllProperties;
            props[propertyBagName] = serializer.Serialize(searchSettings);
        }

        protected virtual SearchSettingsConfig GetCurrentSearchConfigAtWebLevel(Web web)
        {
            return InternalGetCurrentSearchConfig(web, true);
        }

        protected virtual SearchSettingsConfig GetCurrentSearchConfigAtSiteLevel(Web web)
        {
            return InternalGetCurrentSearchConfig(web, false);
        }

        private SearchSettingsConfig InternalGetCurrentSearchConfig(Web web, bool isWebLevel)
        {
            SearchSettingsConfig result = null;
            var serializer = new JavaScriptSerializer();

            var propertyBagName = "SRCH_SB_SET_SITE";

            if (isWebLevel)
                propertyBagName = "SRCH_SB_SET_WEB";

            try
            {
                var rawSearchSettings = string.Empty;

                if (web.AllProperties.FieldValues.ContainsKey(propertyBagName))
                    rawSearchSettings = ConvertUtils.ToStringAndTrim(web.AllProperties[propertyBagName]);

                result = serializer.Deserialize<SearchSettingsConfig>(rawSearchSettings);

                // no setup -> an empty string gives NULL
                // create default one to push the setting 
                if (result == null)
                    result = new SearchSettingsConfig();
            }
            catch (Exception)
            {

            }

            return result;
        }


        #endregion
    }
}
