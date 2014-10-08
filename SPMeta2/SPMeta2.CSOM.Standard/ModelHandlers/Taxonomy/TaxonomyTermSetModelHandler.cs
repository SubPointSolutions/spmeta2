using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy
{
    public class TaxonomyTermSetModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TaxonomyTermSetDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var groupModelHost = modelHost.WithAssertAndCast<TermGroupModelHost>("modelHost", value => value.RequireNotNull());
            var groupModel = model.WithAssertAndCast<TaxonomyTermSetDefinition>("model", value => value.RequireNotNull());

            DeployTaxonomyTermSet(modelHost, groupModelHost, groupModel);

        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var groupModelHost = modelHost.WithAssertAndCast<TermGroupModelHost>("modelHost", value => value.RequireNotNull());
            var termSetModel = model.WithAssertAndCast<TaxonomyTermSetDefinition>("model", value => value.RequireNotNull());

            var currentTermSet = FindTermSet(groupModelHost.HostGroup, termSetModel);

            action(new TermSetModelHost
            {
                HostGroup = groupModelHost.HostGroup,
                HostTermStore = groupModelHost.HostTermStore,
                HostTermSet = currentTermSet
            });
        }

        private void DeployTaxonomyTermSet(object modelHost, TermGroupModelHost groupModelHost, TaxonomyTermSetDefinition termSetModel)
        {
            var termStore = groupModelHost.HostTermStore;
            var termGroup = groupModelHost.HostGroup;

            var currentTermSet = FindTermSet(termGroup, termSetModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentTermSet,
                ObjectType = typeof(TermSet),
                ObjectDefinition = termSetModel,
                ModelHost = modelHost
            });

            if (currentTermSet == null)
            {
                currentTermSet = termSetModel.Id.HasValue
                    ? termGroup.CreateTermSet(termSetModel.Name, termSetModel.Id.Value, termSetModel.LCID)
                    : termGroup.CreateTermSet(termSetModel.Name, Guid.NewGuid(), termSetModel.LCID);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentTermSet,
                    ObjectType = typeof(TermSet),
                    ObjectDefinition = termSetModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentTermSet,
                    ObjectType = typeof(TermSet),
                    ObjectDefinition = termSetModel,
                    ModelHost = modelHost
                });
            }
        }

        protected TermSet FindTermSet(TermGroup termGroup, TaxonomyTermSetDefinition termSetModel)
        {
            TermSet result = null;

            var context = termGroup.Context;

            context.Load(termGroup.TermSets);
            context.ExecuteQuery();

            if (termSetModel.Id.HasValue)
                result = termGroup.TermSets.FirstOrDefault(t => t.Id == termSetModel.Id.Value);
            else if (!string.IsNullOrEmpty(termSetModel.Name))
                result = termGroup.TermSets.FirstOrDefault(t => t.Name.ToUpper() == termSetModel.Name.ToUpper());

            return result;
        }

        #endregion
    }
}
