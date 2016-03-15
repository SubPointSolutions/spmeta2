using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.Config;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class MetadataNavigationSettingsModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(MetadataNavigationSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listHost = modelHost.WithAssertAndCast<ListModelHost>("model", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<MetadataNavigationSettingsDefinition>("model", value => value.RequireNotNull());

            DeploySettings(modelHost, listHost, typedDefinition);
        }

        protected MetadataNavigationSettingsConfig GetCurrentSettings(List list)
        {
            return MetadataNavigationSettingsConfig.GetMetadataNavigationSettings(list);
        }

        private void DeploySettings(object modelHost, ListModelHost listHost, MetadataNavigationSettingsDefinition definition)
        {
            var list = listHost.HostList;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = list,
                ObjectType = typeof(List),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            var needUpdate = false;

            // deploy
            var settings = GetCurrentSettings(list);

            if (definition.Hierarchies.Count() > 0)
            {
                foreach (var h in definition.Hierarchies)
                {
                    if (h.FieldId.HasGuidValue())
                    {
                        var targetField = list.Fields.GetById(h.FieldId.Value);

                        settings.AddConfiguredHierarchy(new MetadataNavigationHierarchyConfig(targetField.Id));
                    }
                }

                needUpdate = true;
            }

            if (definition.KeyFilters.Count() > 0)
            {
                foreach (var h in definition.KeyFilters)
                {
                    if (h.FieldId.HasGuidValue())
                    {
                        var targetField = list.Fields.GetById(h.FieldId.Value);

                        settings.AddConfiguredKeyFilter(new MetadataNavigationKeyFilterConfig(targetField.Id));
                    }
                }

                needUpdate = true;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = list,
                ObjectType = typeof(List),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });


            if (needUpdate)
            {
                MetadataNavigationSettingsConfig.SetMetadataNavigationSettings(list, settings);
            }
        }

        #endregion
    }
}
