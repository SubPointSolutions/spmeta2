using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class DiagnosticsServiceBaseModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DiagnosticsServiceBaseDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DiagnosticsServiceBaseDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, farmModelHost.HostFarm, definition);
        }

        private void DeployDefinition(object modelHost, SPFarm spFarm, DiagnosticsServiceBaseDefinition definition)
        {
            var currentObject = GetCurrentObject(spFarm, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(SPDiagnosticsServiceBase),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentObject == null)
                currentObject = CreateObject(spFarm, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(SPDiagnosticsServiceBase),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        private SPDiagnosticsServiceBase CreateObject(SPFarm spFarm, DiagnosticsServiceBaseDefinition definition)
        {
            var serviceType = Type.GetType(definition.AssemblyQualifiedName);
            var serviceInstance = Activator.CreateInstance(serviceType) as SPDiagnosticsServiceBase;

            spFarm.Services.Add(serviceInstance);
            spFarm.Update();

            return GetCurrentObject(spFarm, definition);
        }

        private SPDiagnosticsServiceBase GetCurrentObject(SPFarm spFarm, DiagnosticsServiceBaseDefinition definition)
        {
            return spFarm.Services.FirstOrDefault(s => s.TypeName.ToUpper() == definition.AssemblyQualifiedName.ToUpper()) as SPDiagnosticsServiceBase;
        }

        #endregion
    }
}
