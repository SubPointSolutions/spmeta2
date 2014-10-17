using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class PropertyModelHandler : CSOMModelHandlerBase
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
            var property = model.WithAssertAndCast<PropertyDefinition>("model", value => value.RequireNotNull());

            DeployProperty(modelHost, properties, property);
        }

        protected PropertyValues ExtractProperties(object modelHost)
        {
            PropertyValues result = null;
            ClientRuntimeContext context = null;

            if (modelHost is SiteModelHost)
            {
                result = (modelHost as SiteModelHost).HostSite.RootWeb.AllProperties;
                context = (modelHost as SiteModelHost).HostSite.RootWeb.Context;
            }
            else if (modelHost is WebModelHost)
            {
                result = (modelHost as WebModelHost).HostWeb.AllProperties;
                context = (modelHost as WebModelHost).HostWeb.Context;
            }
            else if (modelHost is ListModelHost)
            {
                result = (modelHost as ListModelHost).HostList.RootFolder.Properties;
                context = (modelHost as ListModelHost).HostList.RootFolder.Context;
            }
            else if (modelHost is FolderModelHost)
            {
                result = (modelHost as FolderModelHost).CurrentLibraryFolder.Properties;
                context = (modelHost as FolderModelHost).CurrentLibraryFolder.Context;
            }
            else if (modelHost is ListItem)
            {
                // http://officespdev.uservoice.com/forums/224641-general/suggestions/6343086-expose-properties-property-for-microsoft-sharepo

                throw new SPMeta2NotImplementedException("ListItem properties provision is not supported yet.");
                //DeployProperty(host, host.CurrentListItem.all, property);
            }
            else if (modelHost is File)
            {
                // http://officespdev.uservoice.com/forums/224641-general/suggestions/6343087-expose-properties-property-for-microsoft-sharepo

                throw new SPMeta2NotImplementedException("File properties provision is not supported yet.");
                // DeployProperty(host, host.CurrentFile., property);
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("Model host [{0}] is not supported yet.", modelHost));
            }


            context.Load(result);
            context.ExecuteQuery();

            return result;
        }

        protected virtual void DeployProperty(object modelHost, PropertyValues properties, PropertyDefinition property)
        {
            var currentValue = properties.FieldValues.ContainsKey(property.Key) ? properties[property.Key] : null;

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
