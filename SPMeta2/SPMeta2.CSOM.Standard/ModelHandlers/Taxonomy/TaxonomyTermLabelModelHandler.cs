using System;
using System.Linq;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;

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
            SharePointOnlineWait(termModelHost, definition);
        }

        private void SharePointOnlineWait(TermModelHost termModelHost, TaxonomyTermLabelDefinition definition)
        {
            // wait until the group is there
            // Nested terms provisioning in Office 365 fails #995
            // TermSet not found #994
            var context = termModelHost.HostClientContext;

            if (IsSharePointOnlineContext(context))
            {
                var term = termModelHost.HostTerm;
                var currentLabel = FindLabelInTerm(term, definition);

                if (currentLabel == null)
                {
                    TryRetryService.TryWithRetry(() =>
                    {
                        currentLabel = FindLabelInTerm(term, definition);
                        return currentLabel != null;
                    });
                }

                if (currentLabel == null)
                    throw new SPMeta2Exception(string.Format("Cannot find a term label after provision"));
            }
        }
        private void DeployTermLabel(object modelHost, TermModelHost termModelHost, TaxonomyTermLabelDefinition labelModel)
        {
            var termStore = termModelHost.HostTermStore;
            var context = termStore.Context;

            var term = termModelHost.HostTerm;
            var currentLabel = FindLabelInTerm(term, labelModel);

            var shouldCommitChanges = false;

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

                shouldCommitChanges = true;
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

                shouldCommitChanges = false;
            }

            if (shouldCommitChanges)
            {
                termStore.CommitAll();

                currentLabel.RefreshLoad();
                term.RefreshLoad();

                context.ExecuteQueryWithTrace();
            }
        }

        protected Label FindLabelInTerm(Term termSet, TaxonomyTermLabelDefinition labelModel)
        {
            var context = termSet.Context;
            var labels = termSet.Labels;

            context.Load(labels);
            context.ExecuteQueryWithTrace();

            return termSet.Labels.ToList().FirstOrDefault(l => l.Value.ToUpper() == labelModel.Name.ToUpper());
        }

        #endregion
    }
}
