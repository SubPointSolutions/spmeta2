using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Base
{
    public abstract class SingleHostTypedSSOMModelHandlerBase<TModelHostType, TDefinition, TSPObject> : SSOMModelHandlerBase
         where TDefinition : DefinitionBase
    {
        protected abstract TSPObject GetCurrentObject(TModelHostType typedModelHost, TDefinition definition);
        protected abstract void UpdateObject(TSPObject currentObject);

        protected abstract TSPObject CreateObject(TModelHostType typedModelHost, TDefinition definition);

        protected abstract void MapObject(TSPObject currentObject, TDefinition definition);

        public override Type TargetType
        {
            get { return typeof(TDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<TModelHostType>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<TDefinition>("model", value => value.RequireNotNull());

            DeployTypedModel(modelHost, typedModelHost, typedDefinition);
        }

        protected void DeployTypedModel(object modelHost, TModelHostType typedModelHost, TDefinition typedDefinition)
        {
            var currentObject = GetCurrentObject(typedModelHost, typedDefinition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(TSPObject),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            if (currentObject == null)
                currentObject = CreateObject(typedModelHost, typedDefinition);

            MapObject(currentObject, typedDefinition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(TSPObject),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            UpdateObject(currentObject);
        }
    }
}
