using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;

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
            var termSetModel = model.WithAssertAndCast<TaxonomyTermSetDefinition>("model", value => value.RequireNotNull());

            DeployTaxonomyTermSet(modelHost, groupModelHost, termSetModel);
            SharePointOnlineWait(groupModelHost, termSetModel);
        }

        private void SharePointOnlineWait(TermGroupModelHost groupModelHost, TaxonomyTermSetDefinition termSetModel)
        {
            // wait until the group is there
            // Nested terms provisioning in Office 365 fails #995
            // TermSet not found #994
            var context = groupModelHost.HostClientContext;

            if (IsSharePointOnlineContext(context))
            {
                var currentTermSet = FindTermSet(groupModelHost.HostGroup, termSetModel);

                if (currentTermSet == null)
                {
                    TryRetryService.TryWithRetry(() =>
                    {
                        currentTermSet = FindTermSet(groupModelHost.HostGroup, termSetModel);
                        return currentTermSet != null;
                    });
                }

                if (currentTermSet == null)
                    throw new SPMeta2Exception(string.Format("Cannot find a termset after provision"));
            }
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var groupModelHost = modelHost.WithAssertAndCast<TermGroupModelHost>("modelHost", value => value.RequireNotNull());
            var termSetModel = model.WithAssertAndCast<TaxonomyTermSetDefinition>("model", value => value.RequireNotNull());

            var context = groupModelHost.HostClientContext;
            var currentTermSet = FindTermSet(groupModelHost.HostGroup, termSetModel);

            if (currentTermSet == null && IsSharePointOnlineContext(context))
            {
                TryRetryService.TryWithRetry(() =>
                {
                    currentTermSet = FindTermSet(groupModelHost.HostGroup, termSetModel);
                    return currentTermSet != null;
                });
            }

            if (currentTermSet == null)
                throw new SPMeta2Exception(string.Format("Cannot find a taxonomy term set after provision"));

            action(ModelHostBase.Inherit<TermSetModelHost>(groupModelHost, host =>
            {
                host.HostGroup = groupModelHost.HostGroup;
                host.HostTermStore = groupModelHost.HostTermStore;
                host.HostTermSet = currentTermSet;
            }));
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

                UpdateExistingTaxonomyTermSet(modelHost, termSetModel, currentTermSet);
            }

            termStore.CommitAll();

            try
            {
                termStore.Context.ExecuteQueryWithTrace();
            }
            catch (Exception e)
            {
                var context = groupModelHost.HostClientContext;

                if (!IsSharePointOnlineContext(context))
                    throw;

                // SPMeta2 Provisioning Taxonomy Group with CSOM Standard #959
                // https://github.com/SubPointSolutions/spmeta2/issues/959

                // seems that newly created group might not be available for the time being
                // handling that "Group names must be unique." exception
                // trying to find the group and only update description
                var serverException = e as ServerException;

                if (serverException != null
                    && serverException.ServerErrorCode == -2146233088)
                {
                    currentTermSet = FindTermSet(termGroup, termSetModel);

                    if (currentTermSet == null)
                    {
                        TryRetryService.TryWithRetry(() =>
                        {
                            currentTermSet = FindTermSet(termGroup, termSetModel);
                            return currentTermSet != null;
                        });
                    }

                    UpdateExistingTaxonomyTermSet(modelHost, termSetModel, currentTermSet);

                    termStore.CommitAll();
                    termStore.RefreshLoad();

                    termStore.Context.ExecuteQueryWithTrace();
                }
            }

            groupModelHost.ShouldUpdateHost = false;
        }

        private void UpdateExistingTaxonomyTermSet(object modelHost, TaxonomyTermSetDefinition termSetModel, TermSet currentTermSet)
        {
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

        private static void MapTermSet(TermSet currentTermSet, TaxonomyTermSetDefinition termSetModel)
        {
            if (!string.IsNullOrEmpty(termSetModel.Description))
                currentTermSet.Description = termSetModel.Description;

            if (!string.IsNullOrEmpty(termSetModel.Contact))
                currentTermSet.Contact = termSetModel.Contact;

            if (!string.IsNullOrEmpty(termSetModel.CustomSortOrder))
                currentTermSet.CustomSortOrder = termSetModel.CustomSortOrder;

            if (termSetModel.IsOpenForTermCreation.HasValue)
                currentTermSet.IsOpenForTermCreation = termSetModel.IsOpenForTermCreation.Value;

            if (termSetModel.IsAvailableForTagging.HasValue)
                currentTermSet.IsAvailableForTagging = termSetModel.IsAvailableForTagging.Value;


            foreach (var customProp in termSetModel.CustomProperties.Where(p => p.Override))
            {
                currentTermSet.SetCustomProperty(customProp.Name, customProp.Value);
            }
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

            if (result != null
                && result.ServerObjectIsNull.HasValue
                && result.ServerObjectIsNull == false)
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
