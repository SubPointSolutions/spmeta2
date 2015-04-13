using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new Term Set");

                currentTermSet = termSetModel.Id.HasValue
                    ? termGroup.CreateTermSet(termSetModel.Name, termSetModel.Id.Value, termSetModel.LCID)
                    : termGroup.CreateTermSet(termSetModel.Name, Guid.NewGuid(), termSetModel.LCID);

                MapTermSet(currentTermSet, termSetModel);

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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing Term Set");

                MapTermSet(currentTermSet, termSetModel);

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

            termStore.CommitAll();
            termStore.Context.ExecuteQuery();
        }

        private static void MapTermSet(TermSet currentTermSet, TaxonomyTermSetDefinition termSetModel)
        {
            currentTermSet.Description = termSetModel.Description;

            if (termSetModel.IsOpenForTermCreation.HasValue)
                currentTermSet.IsOpenForTermCreation = termSetModel.IsOpenForTermCreation.Value;

            if (termSetModel.IsAvailableForTagging.HasValue)
                currentTermSet.IsAvailableForTagging = termSetModel.IsAvailableForTagging.Value;
        }

        protected TermSet FindTermSet(TermGroup termGroup, TaxonomyTermSetDefinition termSetModel)
        {
            TermSet result = null;

            var context = termGroup.Context;

            context.Load(termGroup.TermSets);
            context.ExecuteQueryWithTrace();

            if (termSetModel.Id.HasValue)
            {
                var scope = new ExceptionHandlingScope(context);
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        result = termGroup.TermSets.GetById(termSetModel.Id.Value);
                        context.Load(result);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }
            }
            else if (!string.IsNullOrEmpty(termSetModel.Name))
            {
                var scope = new ExceptionHandlingScope(context);
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        result = termGroup.TermSets.GetByName(termSetModel.Name);
                        context.Load(result);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }
            }

            context.ExecuteQueryWithTrace();

            if (result != null && result.ServerObjectIsNull == false)
            {
                context.Load(result);
                //context.Load(result, g => g.Id);
                //context.Load(result, g => g.Name);

                context.ExecuteQueryWithTrace();

                return result;
            }

            return null;
        }

        #endregion
    }
}
