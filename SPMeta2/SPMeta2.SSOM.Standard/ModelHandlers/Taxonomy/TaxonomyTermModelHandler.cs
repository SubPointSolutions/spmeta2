using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class TaxonomyTermModelHandler : SSOMModelHandlerBase
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
            Group group = null;
            TermSet termSet = null;
            TermStore termStore = null;

            if (modelHost is TermModelHost)
            {
                var h = (modelHost as TermModelHost);

                group = h.HostGroup;
                termSet = h.HostTermSet;
                termStore = h.HostTermStore;

                currentTerm = FindTermInTerm(h.HostTerm, definition);
            }
            else if (modelHost is TermSetModelHost)
            {
                var h = (modelHost as TermSetModelHost);

                termStore = h.HostTermStore;
                group = h.HostGroup;
                termSet = h.HostTermSet;

                currentTerm = FindTermInTermSet(h.HostTermSet, definition);
            }

            action(new TermModelHost
            {
                HostGroup = group,
                HostTermSet = termSet,
                HostTerm = currentTerm
            });

            //termStore.CommitAll();
        }

        private void DeployTermUnderTerm(object modelHost, TermModelHost termModelHost, TaxonomyTermDefinition termModel)
        {
            var term = termModelHost.HostTerm;

            var currentTerm = FindTermInTerm(term, termModel);

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
                    ? term.CreateTerm(termModel.Name, termModel.LCID, termModel.Id.Value)
                    : term.CreateTerm(termModel.Name, termModel.LCID);

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
        }

        private void DeployTermUnderTermSet(object modelHost, TermSetModelHost groupModelHost, TaxonomyTermDefinition termModel)
        {
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
                    : termSet.CreateTerm(termModel.Name, termModel.LCID);

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

            groupModelHost.HostTermStore.CommitAll();
        }

        private Term FindTermInTerm(Term term, TaxonomyTermDefinition termModel)
        {
            Term result = null;

            // TODO

            if (termModel.Id.HasValue)
                result = term.Terms[termModel.Id.Value];
            else if (!string.IsNullOrEmpty(termModel.Name))
                result = term.Terms.FirstOrDefault(t => t.Name == termModel.Name);

            return result;
        }

        protected Term FindTermInTermSet(TermSet termSet, TaxonomyTermDefinition termModel)
        {
            Term result = null;

            if (termModel.Id.HasValue)
                result = termSet.GetTerm(termModel.Id.Value);
            else if (!string.IsNullOrEmpty(termModel.Name))
                result = termSet.GetTerms(termModel.Name, termModel.LCID, false).FirstOrDefault();

            return result;
        }

        #endregion
    }
}
