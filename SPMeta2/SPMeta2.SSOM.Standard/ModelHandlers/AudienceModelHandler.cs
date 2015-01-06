using System;
using System.Linq;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class AudienceModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(AudienceDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<AudienceDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, siteModelHost, definition);
        }

        protected Audience GetCurrentObject(SiteModelHost siteModelHost, AudienceDefinition definition)
        {
            var site = siteModelHost.HostSite;
            var serverContext = ServerContext.GetContext(site);

            var audienceManager = new AudienceManager(serverContext);
            var audiencies = audienceManager.Audiences;

            try
            {
                return audiencies[definition.AudienceName];
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void UpdateObject(Audience currentObject)
        {
            currentObject.Commit();
        }

        private Audience CreateObject(SiteModelHost siteModelHost, AudienceDefinition definition)
        {
            var site = siteModelHost.HostSite;
            var serverContext = ServerContext.GetContext(site);

            var audienceManager = new AudienceManager(serverContext);
            var audiencies = audienceManager.Audiences;

            var result = audiencies.Create(definition.AudienceName, definition.AudienceDescription);

            return result;
        }

        private void ProcessExistingObject(Audience currentObject, AudienceDefinition definition)
        {
            currentObject.AudienceName = definition.AudienceName;
            currentObject.AudienceDescription = definition.AudienceDescription;
        }

        private void DeployDefinition(object modelHost, SiteModelHost siteModelHost, AudienceDefinition definition)
        {
            var currentObject = GetCurrentObject(siteModelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(Audience),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentObject == null)
                currentObject = CreateObject(siteModelHost, definition);

            ProcessExistingObject(currentObject, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(Audience),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            UpdateObject(currentObject);
        }

        #endregion
    }
}
