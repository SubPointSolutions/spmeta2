using System;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Common;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SiteModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SiteDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            action(siteModelHost);
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var siteModel = model.WithAssertAndCast<SiteDefinition>("model", value => value.RequireNotNull());

            DeploySiteModel(modelHost, siteModelHost.HostSite, siteModel);
        }

        private void DeploySiteModel(object modelHost, Microsoft.SharePoint.Client.Site site, SiteDefinition siteModel)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = site,
                ObjectType = typeof(Site),
                ObjectDefinition = siteModel,
                ModelHost = modelHost
            });

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = site,
                ObjectType = typeof(Site),
                ObjectDefinition = siteModel,
                ModelHost = modelHost
            });
        }



        #endregion
    }
}
