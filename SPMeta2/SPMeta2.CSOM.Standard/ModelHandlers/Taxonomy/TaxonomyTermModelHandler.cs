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
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.Standard.Utils;

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

            // TODO, move to common validator infrastructure
            if (!TaxonomyUtility.IsValidTermName(definition.Name))
            {
                throw new SPMeta2Exception(
                    string.Format("Term name [{0}] cannot contain any of the following characters: \" ; < > | and Tab",
                        definition.Name));
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

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
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
                    ? termSet.CreateTerm(termModel.Name, termModel.LCID, termModel.Id.Value)
                    : termSet.CreateTerm(termModel.Name, termModel.LCID, Guid.NewGuid());

                currentTerm.SetDescription(termModel.Description, termModel.LCID);

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

                currentTerm.SetDescription(termModel.Description, termModel.LCID);

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
            termStore.Context.ExecuteQuery();
        }

        private void DeployTermUnderTerm(object modelHost, TermModelHost groupModelHost, TaxonomyTermDefinition termModel)
        {
            var termStore = groupModelHost.HostTermStore;
            var termSet = groupModelHost.HostTerm;

            var currentTerm = FindTermInTerm(termSet, termModel);

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
                    ? termSet.CreateTerm(termModel.Name, termModel.LCID, termModel.Id.Value)
                    : termSet.CreateTerm(termModel.Name, termModel.LCID, Guid.NewGuid());

                currentTerm.SetDescription(termModel.Description, termModel.LCID);

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

                currentTerm.SetDescription(termModel.Description, termModel.LCID);

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
            termStore.Context.ExecuteQuery();
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
                context.ExecuteQuery();

            }
            else if (!string.IsNullOrEmpty(termModel.Name))
            {
                var name = termModel.Name;

                //var terms = term.Terms;

                //context.Load(terms);
                //context.ExecuteQueryWithTrace();


                results = context.LoadQuery(term.Terms.Where(t => t.Name == name));
                context.ExecuteQuery();
                //result = term.Terms.FirstOrDefault(t => t.Name == termModel.Name);
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
                var terms = termSet.GetTerms(new LabelMatchInformation(context)
                {
                    Lcid = termModel.LCID,
                    TermLabel = termModel.Name,
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
