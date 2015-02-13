using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers.Base
{
    public abstract class PropertyModelHandler : CSOMModelHandlerBase
    {
        #region properties

        #endregion

        #region methods

        protected virtual void DeployProperty(object modelHost, PropertyValues properties, PropertyDefinition propertyDefinition)
        {
            var currentValue = properties.FieldValues.ContainsKey(propertyDefinition.Key) ? properties[propertyDefinition.Key] : null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentValue,
                ObjectType = typeof(object),
                ObjectDefinition = propertyDefinition,
                ModelHost = modelHost
            });

            if (currentValue == null)
            {
                properties[propertyDefinition.Key] = propertyDefinition.Value;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = propertyDefinition.Value,
                    ObjectType = typeof(object),
                    ObjectDefinition = propertyDefinition,
                    ModelHost = modelHost
                });
            }
            else
            {
                if (propertyDefinition.Overwrite)
                {
                    properties[propertyDefinition.Key] = propertyDefinition.Value;

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = propertyDefinition.Value,
                        ObjectType = typeof(object),
                        ObjectDefinition = propertyDefinition,
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
                        Object = propertyDefinition.Value,
                        ObjectType = typeof(object),
                        ObjectDefinition = propertyDefinition,
                        ModelHost = modelHost
                    });
                }
            }
        }



        #endregion
    }
}
