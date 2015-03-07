using System;
using System.Linq;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy
{
    public class TaxonomyTermLabelModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TaxonomyTermLabelDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var termModelHost = modelHost.WithAssertAndCast<TermModelHost>("model", m => m.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermLabelDefinition>("model", value => value.RequireNotNull());

            DeployTermLabel(modelHost, termModelHost, definition);
        }
        private void DeployTermLabel(object modelHost, TermModelHost termModelHost, TaxonomyTermLabelDefinition labelModel)
        {
            var termStore = termModelHost.HostTermStore;
            var context = termStore.Context;

            var term = termModelHost.HostTerm;
            var currentLabel = FindLabelInTerm(term, labelModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentLabel,
                ObjectType = typeof(Label),
                ObjectDefinition = labelModel,
                ModelHost = modelHost
            });

            if (currentLabel == null)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new Label");

                currentLabel = term.CreateLabel(labelModel.Name, labelModel.LCID, labelModel.IsDefault);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentLabel,
                    ObjectType = typeof(Label),
                    ObjectDefinition = labelModel,
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
                    Object = currentLabel,
                    ObjectType = typeof(Label),
                    ObjectDefinition = labelModel,
                    ModelHost = modelHost
                });
            }

            termStore.CommitAll();
            context.ExecuteQuery();
        }

        protected Label FindLabelInTerm(Term termSet, TaxonomyTermLabelDefinition labelModel)
        {
            var context = termSet.Context;
            var labels = termSet.Labels;

            context.Load(labels);
            context.ExecuteQuery();

            return termSet.Labels.ToList().FirstOrDefault(l => l.Value.ToUpper() == labelModel.Name.ToUpper());
        }

        #endregion
    }
}
