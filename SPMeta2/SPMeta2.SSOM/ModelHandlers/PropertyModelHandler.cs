using System;
using System.Collections;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
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
            var properties = ExtractProperties(modelHost);
            var propertyModel = model.WithAssertAndCast<PropertyDefinition>("model", value => value.RequireNotNull());

            DeployProperty(modelHost, properties, propertyModel);
        }

        protected Hashtable ExtractProperties(object modelHost)
        {
            if (modelHost is FarmModelHost)
                return (modelHost as FarmModelHost).HostFarm.Properties;

            if (modelHost is WebApplicationModelHost)
                return (modelHost as WebApplicationModelHost).HostWebApplication.Properties;

            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb.AllProperties;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb.AllProperties;

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.RootFolder.Properties;

            if (modelHost is FolderModelHost)
            {
                var folderModelHost = (modelHost as FolderModelHost);

                if (folderModelHost.CurrentLibraryFolder != null)
                    return folderModelHost.CurrentLibraryFolder.Properties;
                else
                    return folderModelHost.CurrentListItem.Properties;
            }

            if (modelHost is SPListItem)
                return (modelHost as SPListItem).Properties;

            throw new SPMeta2NotSupportedException(string.Format("model host of type [{0}] is not supported.", modelHost.GetType()));
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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new property");

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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing property");

                if (property.Overwrite)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Overwrite = true. Overwriting property.");

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
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Overwrite = false. Skipping property.");


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
