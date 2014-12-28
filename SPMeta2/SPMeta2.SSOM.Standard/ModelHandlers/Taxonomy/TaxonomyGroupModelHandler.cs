using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
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
            var currentGroup = FindGroup(termStore, groupModel);

            action(new TermGroupModelHost
            {
                HostTermStore = termStore,
                HostGroup = currentGroup
            });
        }

        protected Group FindGroup(TermStore termStore, TaxonomyTermGroupDefinition groupModel)
        {
            Group currentGroup = null;

            if (groupModel.Id.HasValue)
                currentGroup = termStore.GetGroup(groupModel.Id.Value);
            else if (!string.IsNullOrEmpty(groupModel.Name))
                currentGroup = termStore.Groups.FirstOrDefault(g => g.Name.ToUpper() == groupModel.Name.ToUpper());

            return currentGroup;
        }

        private void DeployTaxonomyGroup(object modelHost, TermStoreModelHost siteModelHost, TaxonomyTermGroupDefinition groupModel)
        {
            var termStore = siteModelHost.HostTermStore;
            var currentGroup = FindGroup(termStore, groupModel);

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

                currentGroup = groupModel.Id.HasValue
                                        ? termStore.CreateGroup(groupModel.Name, groupModel.Id.Value)
                                        : termStore.CreateGroup(groupModel.Name);

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
