using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Base
{
    public class PropertyModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PropertyDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var propertyHost = modelHost.WithAssertAndCast<PropertyModelHost>("modelHost", value => value.RequireNotNull());
            var property = model.WithAssertAndCast<PropertyDefinition>("model", value => value.RequireNotNull());

            ProcessProperty(propertyHost, property);
        }

        private void ProcessProperty(PropertyModelHost host, PropertyDefinition property)
        {
            if (host.CurrentFarm != null)
                DeployProperty(host, host.CurrentFarm.Properties, property);
            else if (host.CurrentWebApplication != null)
                DeployProperty(host, host.CurrentWebApplication.Properties, property);
            else if (host.CurrentSite != null)
                DeployProperty(host, host.CurrentSite.RootWeb.AllProperties, property);
            else if (host.CurrentWeb != null)
                DeployProperty(host, host.CurrentWeb.AllProperties, property);
            else if (host.CurrentList != null)
                DeployProperty(host, host.CurrentList.RootFolder.Properties, property);
            else if (host.CurrentFolder != null)
                DeployProperty(host, host.CurrentFolder.Properties, property);
            else if (host.CurrentListItem != null)
                DeployProperty(host, host.CurrentListItem.Properties, property);
            else if (host.CurrentFile != null)
                DeployProperty(host, host.CurrentFile.Properties, property);
        }

        protected virtual void DeployProperty(object modelHost, Hashtable properties, PropertyDefinition property)
        {
            var currentValue = properties[property.Key];

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentValue,
                ObjectType = typeof(object),
                ObjectDefinition = property,
                ModelHost = modelHost
            });

            if (currentValue == null)
            {
                properties[property.Key] = property.Value;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = property.Value,
                    ObjectType = typeof(object),
                    ObjectDefinition = property,
                    ModelHost = modelHost
                });
            }
            else
            {
                if (property.Overwrite)
                {
                    properties[property.Key] = property.Value;

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = property.Value,
                        ObjectType = typeof(object),
                        ObjectDefinition = property,
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
                        Object = currentValue,
                        ObjectType = typeof(object),
                        ObjectDefinition = property,
                        ModelHost = modelHost
                    });
                }
            }
        }

        #endregion


    }
}
