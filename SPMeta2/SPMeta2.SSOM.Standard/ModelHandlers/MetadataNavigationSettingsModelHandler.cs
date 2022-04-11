using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.DocumentManagement.MetadataNavigation;
using Microsoft.Office.Server.Search.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using MetadataNavigationHierarchy = Microsoft.Office.DocumentManagement.MetadataNavigation.MetadataNavigationHierarchy;
using MetadataNavigationKeyFilter = Microsoft.Office.DocumentManagement.MetadataNavigation.MetadataNavigationKeyFilter;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class MetadataNavigationSettingsModelHandler : SSOMModelHandlerBase
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

        protected MetadataNavigationSettings GetCurrentSettings(SPList list)
        {
            return MetadataNavigationSettings.GetMetadataNavigationSettings(list);
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
                ObjectType = typeof(SPList),
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
                        var targetField = list.Fields[h.FieldId.Value];

                        settings.AddConfiguredHierarchy(new MetadataNavigationHierarchy(targetField));
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
                        var targetField = list.Fields[h.FieldId.Value];

                        settings.AddConfiguredKeyFilter(new MetadataNavigationKeyFilter(targetField));
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
                ObjectType = typeof(SPList),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });


            if (needUpdate)
            {
                MetadataNavigationSettings.SetMetadataNavigationSettings(list, settings);
            }

        }

        #endregion
    }
}
