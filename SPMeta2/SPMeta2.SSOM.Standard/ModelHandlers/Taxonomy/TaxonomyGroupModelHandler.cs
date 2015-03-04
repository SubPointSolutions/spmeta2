using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHosts;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Taxonomy
{
    public class TaxonomyGroupModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TaxonomyTermGroupDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<TermStoreModelHost>("modelHost", value => value.RequireNotNull());
            var groupModel = model.WithAssertAndCast<TaxonomyTermGroupDefinition>("model", value => value.RequireNotNull());

            DeployTaxonomyGroup(modelHost, siteModelHost, groupModel);
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var storeModelHost = modelHost.WithAssertAndCast<TermStoreModelHost>("modelHost", value => value.RequireNotNull());
            var groupModel = model.WithAssertAndCast<TaxonomyTermGroupDefinition>("model", value => value.RequireNotNull());

            var termStore = storeModelHost.HostTermStore;
            var currentGroup = FindGroup(storeModelHost, groupModel);

            action(new TermGroupModelHost
            {
                HostTermStore = termStore,
                HostGroup = currentGroup
            });
        }

        protected Group FindSiteCollectionGroup(TermStoreModelHost termStoreHost, TaxonomyTermGroupDefinition groupModel)
        {
            var termStore = termStoreHost.HostTermStore;
            return termStore.GetSiteCollectionGroup(termStoreHost.HostSite);
        }

        protected Group FindSystemGroup(TermStoreModelHost termStoreHost, TaxonomyTermGroupDefinition groupModel)
        {
            return termStoreHost.HostTermStore.SystemGroup;
        }

        protected Group FindGroup(TermStoreModelHost termStoreHost, TaxonomyTermGroupDefinition groupModel)
        {
            Group currentGroup = null;

            var termStore = termStoreHost.HostTermStore;

            if (groupModel.IsSiteCollectionGroup)
            {
                currentGroup = FindSiteCollectionGroup(termStoreHost, groupModel);
                return currentGroup;
            }

            //if (groupModel.IsSystemGroup)
            //{
            //    currentGroup = FindSystemGroup(termStoreHost, groupModel);
            //    return currentGroup;
            //}

            if (groupModel.Id.HasValue)
                currentGroup = termStore.GetGroup(groupModel.Id.Value);
            else if (!string.IsNullOrEmpty(groupModel.Name))
                currentGroup = termStore.Groups.FirstOrDefault(g => g.Name.ToUpper() == groupModel.Name.ToUpper());

            return currentGroup;
        }

        private void DeployTaxonomyGroup(object modelHost, TermStoreModelHost siteModelHost, TaxonomyTermGroupDefinition groupModel)
        {
            var termStore = siteModelHost.HostTermStore;
            var currentGroup = FindGroup(siteModelHost, groupModel);
            
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentGroup,
                ObjectType = typeof(Group),
                ObjectDefinition = groupModel,
                ModelHost = modelHost
            });

            if (currentGroup == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new Term Group");

#if !NET35

                currentGroup = groupModel.Id.HasValue
                                        ? termStore.CreateGroup(groupModel.Name, groupModel.Id.Value)
                                        : termStore.CreateGroup(groupModel.Name);

#endif

#if NET35
                // SP2010 API does not support creating groups with particular ID
                currentGroup = termStore.CreateGroup(groupModel.Name);

#endif

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentGroup,
                    ObjectType = typeof(Group),
                    ObjectDefinition = groupModel,
                    ModelHost = modelHost
                });

            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing Term Group");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentGroup,
                    ObjectType = typeof(Group),
                    ObjectDefinition = groupModel,
                    ModelHost = modelHost
                });
            }

            siteModelHost.HostTermStore.CommitAll();
        }

        #endregion
    }
}
