using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class RootWebModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(RootWebDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var currentObject = GetCurrentObject(siteModelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            currentObject.Update();
        }

        protected SPWeb GetCurrentObject(SiteModelHost siteModelHost, RootWebDefinition definition)
        {
            return siteModelHost.HostSite.RootWeb;
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var currentObject = GetCurrentObject(siteModelHost, definition);

            action(new WebModelHost
            {
                HostWeb = currentObject
            });

            currentObject.Update();
        }

        #endregion
    }
}
