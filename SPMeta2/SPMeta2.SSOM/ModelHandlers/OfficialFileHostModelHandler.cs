using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;

using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using System.IO;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class OfficialFileHostModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(OfficialFileHostDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<OfficialFileHostDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, typedModelHost, typedDefinition);
        }

        private void DeployDefinition(object modelHost,
            WebApplicationModelHost typedModelHost,
            OfficialFileHostDefinition typedDefinition)
        {
            var currentObject = FindExistingObject(typedModelHost.HostWebApplication, typedDefinition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(SPOfficialFileHost),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            if (currentObject == null)
            {
                currentObject = CreateNewObject(typedModelHost.HostWebApplication, typedDefinition);
                MapObject(currentObject, typedDefinition);
            }
            else
            {
                MapObject(currentObject, typedDefinition);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(SPOfficialFileHost),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            typedModelHost.HostWebApplication.Update();
        }

        private void MapObject(SPOfficialFileHost currentObject, OfficialFileHostDefinition typedDefinition)
        {
            currentObject.OfficialFileUrl = new Uri(typedDefinition.OfficialFileUrl);
            currentObject.OfficialFileName = typedDefinition.OfficialFileName;
            currentObject.ShowOnSendToMenu = typedDefinition.ShowOnSendToMenu;

            if (!string.IsNullOrEmpty(typedDefinition.Explanation))
                currentObject.Explanation = typedDefinition.Explanation;

            currentObject.Action = (SPOfficialFileAction)typedDefinition.Action;
        }

        private SPOfficialFileHost CreateNewObject(SPWebApplication webApp,
            OfficialFileHostDefinition typedDefinition)
        {
            SPOfficialFileHost result = null;

            if (typedDefinition.CreateUniqueId)
                result = new SPOfficialFileHost(true);
            else
                result = new SPOfficialFileHost(false);

            webApp.OfficialFileHosts.Add(result);

            return result;
        }

        protected virtual SPOfficialFileHost FindExistingObject(SPWebApplication webApp,
            OfficialFileHostDefinition definition)
        {
            return webApp.OfficialFileHosts
                         .FirstOrDefault(h => !string.IsNullOrEmpty(h.OfficialFileName)
                         && h.OfficialFileName.ToUpper() == definition.OfficialFileName.ToUpper());
        }

        #endregion
    }
}
