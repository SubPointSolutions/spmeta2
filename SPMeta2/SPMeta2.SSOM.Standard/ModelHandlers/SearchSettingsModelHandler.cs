using System;
using System.Linq;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using System.Web.Script.Serialization;
using SPMeta2.Config;
using SPMeta2.Services;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class SearchSettingsModelHandler : SSOMModelHandlerBase
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

        protected virtual string GetSearchCenterUrlAtWebLevel(SPWeb web)
        {
            return InternalGetSearchCenterUrl(web, true);
        }

        protected virtual string GetSearchCenterUrlAtSiteLevel(SPWeb web)
        {
            return InternalGetSearchCenterUrl(web, false);
        }

        private string InternalGetSearchCenterUrl(SPWeb web, bool isWebLevel)
        {
            var propertyBagName = "SRCH_ENH_FTR_URL_SITE";

            if (isWebLevel)
                propertyBagName = "SRCH_ENH_FTR_URL_WEB";

            return ConvertUtils.ToString(web.AllProperties[propertyBagName]);
        }

        protected virtual void SetSearchCenterUrlAtWebLevel(SPWeb web, string url)
        {
            InternalSetSearchCenterUrl(web, url, true);
        }

        protected virtual void SetSearchCenterUrlAtSiteLevel(SPWeb web, string url)
        {
            InternalSetSearchCenterUrl(web, url, false);
        }

        protected void InternalSetSearchCenterUrl(SPWeb web, string url, bool isWebLevel)
        {
            var propertyBagName = "SRCH_ENH_FTR_URL_SITE";

            if (isWebLevel)
                propertyBagName = "SRCH_ENH_FTR_URL_WEB";

            if (!string.IsNullOrEmpty(url))
            {
                url = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Context = web,
                    Value = url
                }).Value;

                web.AllProperties[propertyBagName] = url;
            }
        }

        private void DeployAtWebLevel(object modelHost, SPWeb web, SearchSettingsDefinition definition)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(definition.SearchCenterUrl))
            {
                SetSearchCenterUrlAtWebLevel(web, definition.SearchCenterUrl);
            }

            var searchSettings = GetCurrentSearchConfigAtWebLevel(web);

            if (searchSettings != null)
            {
                if (definition.UseParentResultsPageUrl.HasValue)
                    searchSettings.Inherit = definition.UseParentResultsPageUrl.Value;

                if (!string.IsNullOrEmpty(definition.UseCustomResultsPageUrl))
                {
                    var url = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Context = web,
                        Value = definition.UseCustomResultsPageUrl
                    }).Value;

                    searchSettings.ResultsPageAddress = url;
                }

                SetCurrentSearchConfigAtWebLevel(web, searchSettings);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            web.Update();
        }

        private void DeployAtSiteLevel(object modelHost, SPSite site, SearchSettingsDefinition definition)
        {
            var web = site.RootWeb;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(definition.SearchCenterUrl))
            {
                SetSearchCenterUrlAtSiteLevel(web, definition.SearchCenterUrl);
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
                        Context = web,
                        Value = definition.UseCustomResultsPageUrl
                    }).Value;


                    searchSettings.ResultsPageAddress = url;
                }

                SetCurrentSearchConfigAtSiteLevel(web, searchSettings);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            web.Update();

        }

        protected virtual void SetCurrentSearchConfigAtWebLevel(SPWeb web, SearchSettingsConfig searchSettings)
        {
            InternalSetCurrentSearchConfig(web, searchSettings, true);
        }

        protected virtual void SetCurrentSearchConfigAtSiteLevel(SPWeb web, SearchSettingsConfig searchSettings)
        {
            InternalSetCurrentSearchConfig(web, searchSettings, false);
        }

        private void InternalSetCurrentSearchConfig(SPWeb web,
            SearchSettingsConfig searchSettings, bool isWebLevel)
        {
            var propertyBagName = "SRCH_SB_SET_SITE";

            if (isWebLevel)
                propertyBagName = "SRCH_SB_SET_WEB";

            var serializer = new JavaScriptSerializer();
            web.AllProperties[propertyBagName] = serializer.Serialize(searchSettings);
        }

        protected virtual SearchSettingsConfig GetCurrentSearchConfigAtWebLevel(SPWeb web)
        {
            return InternalGetCurrentSearchConfig(web, true);
        }

        protected virtual SearchSettingsConfig GetCurrentSearchConfigAtSiteLevel(SPWeb web)
        {
            return InternalGetCurrentSearchConfig(web, false);
        }

        private SearchSettingsConfig InternalGetCurrentSearchConfig(SPWeb web, bool isWebLevel)
        {
            SearchSettingsConfig result = null;
            var serializer = new JavaScriptSerializer();

            var propertyBagName = "SRCH_SB_SET_SITE";

            if (isWebLevel)
                propertyBagName = "SRCH_SB_SET_WEB";

            try
            {
                var rawSearchSettings = ConvertUtils.ToStringAndTrim(web.AllProperties[propertyBagName]);
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
