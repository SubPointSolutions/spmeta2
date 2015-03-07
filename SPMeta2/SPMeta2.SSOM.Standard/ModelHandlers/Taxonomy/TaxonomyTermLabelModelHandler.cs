using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class TaxonomyTermLabelModelHandler : SSOMModelHandlerBase
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
        }

        protected Label FindLabelInTerm(Term termSet, TaxonomyTermLabelDefinition labelModel)
        {
            var result = termSet.Labels.FirstOrDefault(l => l.Value.ToUpper() == labelModel.Name.ToUpper());

            return result;
        }

        #endregion
    }
}
