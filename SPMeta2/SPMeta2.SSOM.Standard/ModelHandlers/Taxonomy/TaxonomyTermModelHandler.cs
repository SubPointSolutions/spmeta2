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
using SPMeta2.Standard.Utils;
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

            if (!TaxonomyUtility.IsValidTermName(definition.Name))
            {
                throw new SPMeta2Exception(
                    string.Format("Term name [{0}] cannot contain any of the following characters: {1}",
                        definition.Name,
                        string.Join(", ", TaxonomyUtility.InvalidTermNameStrings.ToArray())));
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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var definition = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            Term currentTerm = null;
            Group group = null;
            TermSet termSet = null;
            TermStore termStore = null;
            SPSite hostSite = null;

            if (modelHost is TermModelHost)
            {
                var h = (modelHost as TermModelHost);

                group = h.HostGroup;
                termSet = h.HostTermSet;
                termStore = h.HostTermStore;
                hostSite = h.HostSite;

                currentTerm = FindTermInTerm(h.HostTerm, definition);
            }
            else if (modelHost is TermSetModelHost)
            {
                var h = (modelHost as TermSetModelHost);

                termStore = h.HostTermStore;
                group = h.HostGroup;
                termSet = h.HostTermSet;
                hostSite = h.HostSite;

                currentTerm = FindTermInTermSet(h.HostTermSet, definition);
            }

            action(new TermModelHost
            {
                HostGroup = group,
                HostTermSet = termSet,
                HostTerm = currentTerm,
                HostTermStore = termStore,
                HostSite = hostSite
            });

            termStore.CommitAll();
        }



        protected virtual string NormalizeTermName(string termName)
        {
            return TaxonomyUtility.NormalizeName(termName);
        }

        private void DeployTermUnderTerm(object modelHost, TermModelHost termModelHost, TaxonomyTermDefinition termModel)
        {
            var term = termModelHost.HostTerm;

            var currentTerm = FindTermInTerm(term, termModel);
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
                    ? term.CreateTerm(termName, termModel.LCID, termModel.Id.Value)
                    : term.CreateTerm(termName, termModel.LCID);

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

            termModelHost.HostTermStore.CommitAll();
        }

        protected void MapTermProperties(Term currentTerm, TaxonomyTermDefinition termModel)
        {
            if (!string.IsNullOrEmpty(termModel.Description))
                currentTerm.SetDescription(termModel.Description, termModel.LCID);

            if (!string.IsNullOrEmpty(termModel.CustomSortOrder))
                currentTerm.CustomSortOrder = termModel.CustomSortOrder;

            if (termModel.IsAvailableForTagging.HasValue)
                currentTerm.IsAvailableForTagging = termModel.IsAvailableForTagging.Value;

            UpdateTermProperties(currentTerm, termModel, false);
            UpdateTermProperties(currentTerm, termModel, true);
        }

        private void UpdateTermProperties(Term currentTerm, TaxonomyTermDefinition termModel, bool isLocalProperties)
        {
#if !NET35
            // TaxonomyTermCustomProperty.Overwrite set to false - No properties are created. #932
            // https://github.com/SubPointSolutions/spmeta2/issues/932

            // 1 - set everything what is not there
            var srcProperties = termModel.CustomProperties;
            var dstProperties = currentTerm.CustomProperties;

            if (isLocalProperties)
            {
                TraceService.Information((int)LogEventId.ModelProvision, "Processing local custom properties");

                srcProperties = termModel.LocalCustomProperties;
                dstProperties = currentTerm.LocalCustomProperties;
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvision, "Processing custom properties");

                srcProperties = termModel.CustomProperties;
                dstProperties = currentTerm.CustomProperties;
            }

            foreach (var prop in srcProperties)
            {
                var propName = prop.Name;
                var propValue = prop.Value;


                var propExist = dstProperties.Keys
                                             .FirstOrDefault(k => k.ToUpper() == propName.ToUpper())
                                             != null;

                if (!propExist)
                {
                    TraceService.Information((int)LogEventId.ModelProvision,
                                string.Format("Property [{0}] does not exist. Adding with value:[{1}]",
                                new object[] { prop.Name, prop.Value }));

                    if (isLocalProperties)
                        currentTerm.SetLocalCustomProperty(propName, propValue);
                    else
                        currentTerm.SetCustomProperty(propName, propValue);
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvision,
                                string.Format("Property [{0}] exists. No need to add it. Optionally, it will be owerwritten if .Override is set 'true'",
                                new object[] { propName, propValue }));
                }
            }

            // 2 - override as needed
            foreach (var prop in srcProperties.Where(p => p.Override))
            {
                var propName = prop.Name;
                var propValue = prop.Value;

                TraceService.Information((int)LogEventId.ModelProvision,
                                string.Format("Overwriting property [{0}] with value:[{1}] as .Override is set 'true'",
                                new object[] { propName, propValue }));

                if (isLocalProperties)
                    currentTerm.SetLocalCustomProperty(propName, propValue);
                else
                    currentTerm.SetCustomProperty(propName, propValue);
            }
#endif
        }

        private void DeployTermUnderTermSet(object modelHost, TermSetModelHost groupModelHost, TaxonomyTermDefinition termModel)
        {
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
                    : termSet.CreateTerm(termName, termModel.LCID);

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

            groupModelHost.HostTermStore.CommitAll();
        }

        private Term FindTermInTerm(Term term, TaxonomyTermDefinition termModel)
        {
            Term result = null;

            if (termModel.Id.HasValue)
                result = term.Terms.FirstOrDefault(t => t.Id == termModel.Id.Value);
            else if (!string.IsNullOrEmpty(termModel.Name))
            {
                var termName = NormalizeTermName(termModel.Name);
                result = term.Terms.FirstOrDefault(t => t.Name == termName);
            }

            return result;
        }

        protected Term FindTermInTermSet(TermSet termSet, TaxonomyTermDefinition termModel)
        {
            Term result = null;

            if (termModel.Id.HasValue)
                result = termSet.GetTerm(termModel.Id.Value);
            else if (!string.IsNullOrEmpty(termModel.Name))
            {
                var termName = NormalizeTermName(termModel.Name);
                result = termSet.GetTerms(termName, termModel.LCID, false).FirstOrDefault();
            }

            return result;
        }

        #endregion
    }
}
