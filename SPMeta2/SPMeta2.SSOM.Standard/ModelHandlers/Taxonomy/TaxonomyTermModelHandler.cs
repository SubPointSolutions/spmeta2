using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
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
            var termSetMModelHost = modelHost.WithAssertAndCast<TermSetModelHost>("modelHost", value => value.RequireNotNull());
            var termModel = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            DeployTaxonomyTerm(modelHost, termSetMModelHost, termModel);

        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var groupModelHost = modelHost.WithAssertAndCast<TermSetModelHost>("modelHost", value => value.RequireNotNull());
            var termSetModel = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            var currentTermSet = FindTerm(groupModelHost.HostTermSet, termSetModel);

            action(new TermModelHost
            {
                HostGroup = groupModelHost.HostGroup,
                HostTermSet = groupModelHost.HostTermSet,
                HostTerm = currentTermSet
            });
        }

        private void DeployTaxonomyTerm(object modelHost, TermSetModelHost groupModelHost, TaxonomyTermDefinition termModel)
        {
            var termStore = groupModelHost.HostTermStore;
            var termSet = groupModelHost.HostTermSet;

            var currentTerm = FindTerm(termSet, termModel);

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
                currentTerm = termModel.Id.HasValue
                    ? termSet.CreateTerm(termModel.Name, termModel.LCID, termModel.Id.Value)
                    : termSet.CreateTerm(termModel.Name, termModel.LCID);

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

        protected Term FindTerm(TermSet termSet, TaxonomyTermDefinition termModel)
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
