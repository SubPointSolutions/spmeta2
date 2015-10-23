using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Utils;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy
{
    public class TaxonomyTermModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TaxonomyTermDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            if (!TaxonomyUtility.IsValidTermName(definition.Name))
            {
                throw new SPMeta2Exception(
                    string.Format("Term name [{0}] cannot contain any of the following characters: {1}",
                        definition.Name,
                        string.Join(", ", TaxonomyUtility.InvalidTermNameStrings)));
            }

            if (modelHost is TermModelHost)
                DeployTermUnderTerm(modelHost, modelHost as TermModelHost, definition);
            else if (modelHost is TermSetModelHost)
                DeployTermUnderTermSet(modelHost, modelHost as TermSetModelHost, definition);
            else
            {
                throw new SPMeta2UnsupportedModelHostException(string.Format("Model host of type: [{0}] is not supported", modelHost.GetType()));
            }
        }

        protected virtual string NormalizeTermName(string termName)
        {
            return TaxonomyUtility.NormalizeName(termName);
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var definition = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            Term currentTerm = null;
            TermGroup group = null;
            TermSet termSet = null;
            TermStore termStore = null;

            TermModelHost localModelHost = new TermModelHost();

            if (modelHost is TermModelHost)
            {
                var h = (modelHost as TermModelHost);

                group = h.HostGroup;
                termSet = h.HostTermSet;
                termStore = h.HostTermStore;

                currentTerm = FindTermInTerm(h.HostTerm, definition);

                localModelHost.HostGroup = group;
                localModelHost.HostTermSet = termSet;
                localModelHost.HostTerm = currentTerm;
                localModelHost.HostTermStore = termStore;
            }
            else if (modelHost is TermSetModelHost)
            {
                var h = (modelHost as TermSetModelHost);

                termStore = h.HostTermStore;
                group = h.HostGroup;
                termSet = h.HostTermSet;

                currentTerm = FindTermInTermSet(h.HostTermSet, definition);

                localModelHost.HostGroup = group;
                localModelHost.HostTermSet = termSet;
                localModelHost.HostTerm = currentTerm;
                localModelHost.HostTermStore = termStore;
            }

            action(localModelHost);
        }

        private void DeployTermUnderTermSet(object modelHost, TermSetModelHost groupModelHost, TaxonomyTermDefinition termModel)
        {
            var termStore = groupModelHost.HostTermStore;
            var termSet = groupModelHost.HostTermSet;

            var currentTerm = FindTermInTermSet(termSet, termModel);
            var termName = NormalizeTermName(termModel.Name);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentTerm,
                ObjectType = typeof(Term),
                ObjectDefinition = termModel,
                ModelHost = modelHost
            });

            if (currentTerm == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new Term");

                currentTerm = termModel.Id.HasValue
                    ? termSet.CreateTerm(termName, termModel.LCID, termModel.Id.Value)
                    : termSet.CreateTerm(termName, termModel.LCID, Guid.NewGuid());

                MapTermProperties(currentTerm, termModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentTerm,
                    ObjectType = typeof(Term),
                    ObjectDefinition = termModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing Term");

                MapTermProperties(currentTerm, termModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentTerm,
                    ObjectType = typeof(Term),
                    ObjectDefinition = termModel,
                    ModelHost = modelHost
                });
            }

            termStore.CommitAll();
            termStore.Context.ExecuteQueryWithTrace();
        }

        private void MapTermProperties(Term currentTerm, TaxonomyTermDefinition termModel)
        {
            if (!string.IsNullOrEmpty(termModel.Description))
                currentTerm.SetDescription(termModel.Description, termModel.LCID);

            if (!string.IsNullOrEmpty(termModel.CustomSortOrder))
                currentTerm.CustomSortOrder = termModel.CustomSortOrder;

            if (termModel.IsAvailableForTagging.HasValue)
                currentTerm.IsAvailableForTagging = termModel.IsAvailableForTagging.Value;


            foreach (var customProp in termModel.CustomProperties.Where(p => p.Override))
            {
                currentTerm.SetCustomProperty(customProp.Name, customProp.Value);
            }

            foreach (var customProp in termModel.LocalCustomProperties.Where(p => p.Override))
            {
                currentTerm.SetLocalCustomProperty(customProp.Name, customProp.Value);
            }
        }

        private void DeployTermUnderTerm(object modelHost, TermModelHost groupModelHost, TaxonomyTermDefinition termModel)
        {
            var termStore = groupModelHost.HostTermStore;
            var termSet = groupModelHost.HostTerm;

            var currentTerm = FindTermInTerm(termSet, termModel);
            var termName = NormalizeTermName(termModel.Name);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentTerm,
                ObjectType = typeof(Term),
                ObjectDefinition = termModel,
                ModelHost = modelHost
            });

            if (currentTerm == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new Term");

                currentTerm = termModel.Id.HasValue
                    ? termSet.CreateTerm(termName, termModel.LCID, termModel.Id.Value)
                    : termSet.CreateTerm(termName, termModel.LCID, Guid.NewGuid());

                MapTermProperties(currentTerm, termModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentTerm,
                    ObjectType = typeof(Term),
                    ObjectDefinition = termModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing Term");

                MapTermProperties(currentTerm, termModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentTerm,
                    ObjectType = typeof(Term),
                    ObjectDefinition = termModel,
                    ModelHost = modelHost
                });
            }

            termStore.CommitAll();
            termStore.Context.ExecuteQueryWithTrace();
        }


        protected Term FindTermInTerm(Term term, TaxonomyTermDefinition termModel)
        {
            Term result = null;
            IEnumerable<Term> results = null;

            var context = term.Context;
            // TODO

            if (termModel.Id.HasValue)
            {
                var id = termModel.Id.Value;

                results = context.LoadQuery(term.Terms.Where(t => t.Id == id));
                context.ExecuteQueryWithTrace();

            }
            else if (!string.IsNullOrEmpty(termModel.Name))
            {
                var termName = NormalizeTermName(termModel.Name);

                results = context.LoadQuery(term.Terms.Where(t => t.Name == termName));
                context.ExecuteQueryWithTrace();
            }

            result = results.FirstOrDefault();

            if (result != null)
            {
                context.Load(result);
                context.ExecuteQueryWithTrace();

                return result;
            }

            return null;
        }

        protected Term FindTermInTermSet(TermSet termSet, TaxonomyTermDefinition termModel)
        {
            Term result = null;

            var context = termSet.Context;

            if (termModel.Id.HasValue)
            {
                var scope = new ExceptionHandlingScope(context);
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        result = termSet.Terms.GetById(termModel.Id.Value);
                        context.Load(result);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }

                context.ExecuteQueryWithTrace();
            }
            else if (!string.IsNullOrEmpty(termModel.Name))
            {
                var termName = NormalizeTermName(termModel.Name);

                var terms = termSet.GetTerms(new LabelMatchInformation(context)
                {
                    Lcid = termModel.LCID,
                    TermLabel = termName,
                    TrimUnavailable = false
                });

                context.Load(terms);
                context.ExecuteQueryWithTrace();

                result = terms.FirstOrDefault();
            }

            if (result != null && result.ServerObjectIsNull == false)
            {
                context.Load(result);
                context.ExecuteQueryWithTrace();

                return result;
            }

            return null;
        }

        #endregion
    }
}
