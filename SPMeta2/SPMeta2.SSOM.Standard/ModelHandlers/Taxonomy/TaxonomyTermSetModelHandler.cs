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
    public class TaxonomyTermSetModelHandler : SSOMModelHandlerBase
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
                currentTermSet = termSetModel.Id.HasValue
                    ? termGroup.CreateTermSet(termSetModel.Name, termSetModel.Id.Value, termSetModel.LCID)
                    : termGroup.CreateTermSet(termSetModel.Name, termSetModel.LCID);

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
        }

        protected TermSet FindTermSet(Microsoft.SharePoint.Taxonomy.Group termGroup, TaxonomyTermSetDefinition termSetModel)
        {
            TermSet result = null;

            if (termSetModel.Id.HasValue)
                result = termGroup.TermSets.FirstOrDefault(t => t.Id == termSetModel.Id.Value);
            else if (!string.IsNullOrEmpty(termSetModel.Name))
                result = termGroup.TermSets.FirstOrDefault(t => t.Name.ToUpper() == termSetModel.Name.ToUpper());

            return result;
        }

        #endregion
    }
}
