using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.Search.Query;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class ManagedPropertyModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ManagedPropertyDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var managedProperty = model.WithAssertAndCast<ManagedPropertyDefinition>("model", value => value.RequireNotNull());

            if (modelHost is SiteModelHost)
                DeploySiteManagedProperty(modelHost, modelHost as SiteModelHost, managedProperty);
            else if (modelHost is FarmModelHost)
                DeployFarmManagedProperty(modelHost, modelHost as FarmModelHost, managedProperty);
            else
                throw new SPMeta2UnsupportedModelHostException(string.Format("Only SiteModelHost/FarmModelHost are supported"));
        }

        private void DeploySiteManagedProperty(object modelHost, SiteModelHost siteModelHost, ManagedPropertyDefinition managedProperty)
        {
            throw new NotImplementedException();
        }

        protected ManagedProperty GetCurrentObject(object modelHost, ManagedPropertyDefinition definition)
        {
            ManagedPropertyCollection props;
            List<CrawledPropertyInfo> crawledProps;

            return GetCurrentObject(modelHost, definition, out props, out crawledProps);
        }

        protected ManagedProperty GetCurrentObject(object modelHost, ManagedPropertyDefinition definition,
            out ManagedPropertyCollection properties,
            out List<CrawledPropertyInfo> crawledProps)
        {
            if (modelHost is FarmModelHost)
            {
                var context = SPServiceContext.GetContext(SPServiceApplicationProxyGroup.Default,
                    SPSiteSubscriptionIdentifier.Default);
                var searchProxy =
                    context.GetDefaultProxy(typeof(SearchServiceApplicationProxy)) as SearchServiceApplicationProxy;

                var ssai = searchProxy.GetSearchServiceApplicationInfo();
                var application = SearchService.Service.SearchApplications.GetValue<SearchServiceApplication>(ssai.SearchServiceApplicationId);

                SearchObjectOwner searchOwner = new SearchObjectOwner(SearchObjectLevel.Ssa);

                if (cachedCrawledProps == null)
                    cachedCrawledProps = application.GetAllCrawledProperties(string.Empty, string.Empty, 0, searchOwner);

                crawledProps = cachedCrawledProps;

                var schema = new Schema(application);

                properties = schema.AllManagedProperties;
                return properties.FirstOrDefault(p => p.Name.ToUpper() == definition.Name.ToUpper());
            }

            throw new NotImplementedException();
        }

        private static List<CrawledPropertyInfo> cachedCrawledProps;

        private void DeployFarmManagedProperty(object modelHost, FarmModelHost farmModelHost, ManagedPropertyDefinition definition)
        {
            farmModelHost.ShouldUpdateHost = false;

            ManagedPropertyCollection properties;
            List<CrawledPropertyInfo> crawledProps;

            var existingProperty = GetCurrentObject(modelHost, definition, out properties, out crawledProps);

            var isNewMapping = existingProperty == null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingProperty,
                ObjectType = typeof(ManagedProperty),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (existingProperty == null)
            {
                existingProperty = properties.Create(definition.Name, (ManagedDataType)Enum.Parse(typeof(ManagedDataType), definition.ManagedType));
            }

            existingProperty.Description = definition.Description ?? string.Empty;

            if (definition.Searchable.HasValue)
                existingProperty.HasMultipleValues = definition.Searchable.Value;

            if (definition.Queryable.HasValue)
                existingProperty.Queryable = definition.Queryable.Value;

            if (definition.Retrievable.HasValue)
                existingProperty.Retrievable = definition.Retrievable.Value;

            if (definition.HasMultipleValues.HasValue)
                existingProperty.HasMultipleValues = definition.HasMultipleValues.Value;

            if (definition.Refinable.HasValue)
                existingProperty.Refinable = definition.Refinable.Value;

            if (definition.Sortable.HasValue)
                existingProperty.Sortable = definition.Sortable.Value;

            if (definition.SafeForAnonymous.HasValue)
                existingProperty.SafeForAnonymous = definition.SafeForAnonymous.Value;

            if (definition.TokenNormalization.HasValue)
                existingProperty.TokenNormalization = definition.TokenNormalization.Value;


            //if (isNewMapping)
            //{
            var mappings = existingProperty.GetMappings();



            foreach (var managedPropertyMappping in definition.Mappings)
            {
                var crawledProp = crawledProps
                    .FirstOrDefault(p => p.Name.ToUpper() == managedPropertyMappping.CrawledPropertyName.ToUpper());

                var mapping = new Mapping
                {
                    CrawledPropertyName = crawledProp.Name,
                    ManagedPid = existingProperty.PID,
                };

                if (crawledProp != null)
                    mapping.CrawledPropset = crawledProp.Propset;

                mappings.Add(mapping);
            }

            existingProperty.SetMappings(mappings);
            //}

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingProperty,
                ObjectType = typeof(ManagedProperty),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            // Write the changes back
            existingProperty.Update();
        }


        #endregion
    }
}
