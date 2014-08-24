using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WebPropertyModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebPropertyDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var webProperty = model.WithAssertAndCast<WebPropertyDefinition>("model", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;

            DeploytWebProperty(webModelHost, web, webProperty);
        }

        private void DeploytWebProperty(WebModelHost webModelHost, Microsoft.SharePoint.Client.Web web, Definitions.WebPropertyDefinition webProperty)
        {
            var clientContext = web.Context;

            clientContext.Load(web, w => w.AllProperties);
            clientContext.ExecuteQuery();

            var allProperties = web.AllProperties;
            clientContext.Load(allProperties);

            var currentValue = web.AllProperties.FieldValues.ContainsKey(webProperty.Key) ? web.AllProperties[webProperty.Key] : null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentValue,
                ObjectType = typeof(object),
                ObjectDefinition = webProperty,
                ModelHost = webModelHost
            });

            if (currentValue == null)
            {
                web.AllProperties[webProperty.Key] = webProperty.Value;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = webProperty.Value,
                    ObjectType = typeof(object),
                    ObjectDefinition = webProperty,
                    ModelHost = webModelHost
                });

                web.Update();
                clientContext.ExecuteQuery();
            }
            else
            {
                if (webProperty.Overwrite)
                {
                    web.AllProperties[webProperty.Key] = webProperty.Value;

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = webProperty.Value,
                        ObjectType = typeof(object),
                        ObjectDefinition = webProperty,
                        ModelHost = webModelHost
                    });

                    web.Update();
                    clientContext.ExecuteQuery();
                }
                else
                {
                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = webProperty.Value,
                        ObjectType = typeof(object),
                        ObjectDefinition = webProperty,
                        ModelHost = webModelHost
                    });

                    web.Update();
                    clientContext.ExecuteQuery();
                }
            }
        }



        #endregion
    }
}
