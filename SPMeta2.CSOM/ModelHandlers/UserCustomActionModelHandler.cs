using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class UserCustomActionModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UserCustomActionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (!IsValidHostModelHost(modelHost))
                throw new Exception(string.Format("modelHost of type {0} is not supported.", modelHost.GetType()));

            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var customAction = model.WithAssertAndCast<UserCustomActionDefinition>("model", value => value.RequireNotNull());


            DeploySiteCustomAction(siteModelHost, customAction);
        }

        private void DeploySiteCustomAction(SiteModelHost modelHost, UserCustomActionDefinition model)
        {
            var site = modelHost.HostSite;
            var context = site.Context;

            context.Load(site, s => s.UserCustomActions);
            context.ExecuteQuery();

            var existingAction = site.UserCustomActions.FirstOrDefault(a => a.Name == model.Name) ??
                                  site.UserCustomActions.Add();

            InvokeOnModelEvents(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(UserCustomAction),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            MapCustomAction(existingAction, model);

            InvokeOnModelEvents(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingAction,
                ObjectType = typeof(UserCustomAction),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            existingAction.Update();

            context.ExecuteQuery();
        }

        private void MapCustomAction(UserCustomAction existringAction, UserCustomActionDefinition customAction)
        {
            existringAction.Description = customAction.Description;
            existringAction.Group = customAction.Group;
            existringAction.Location = customAction.Location;
            existringAction.Name = customAction.Name;
            existringAction.ScriptBlock = customAction.ScriptBlock;
            existringAction.ScriptSrc = customAction.ScriptSrc;
            existringAction.Title = customAction.Title;
        }

        private bool IsValidHostModelHost(object modelHost)
        {
            return modelHost is SiteModelHost;
        }

        #endregion
    }
}
