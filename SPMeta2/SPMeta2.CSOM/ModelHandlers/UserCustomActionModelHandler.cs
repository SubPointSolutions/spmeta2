using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
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

        protected UserCustomAction GetCustomAction(SiteModelHost modelHost, UserCustomActionDefinition model)
        {
            var site = modelHost.HostSite;
            var context = site.Context;

            context.Load(site, s => s.UserCustomActions);
            context.ExecuteQueryWithTrace();

            return site.UserCustomActions.FirstOrDefault(a => a.Name == model.Name);
        }

        private void DeploySiteCustomAction(SiteModelHost modelHost, UserCustomActionDefinition model)
        {
            var site = modelHost.HostSite;
            var context = site.Context;

            var existingAction = GetCustomAction(modelHost, model);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(UserCustomAction),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (existingAction == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new user custom action");
                existingAction = site.UserCustomActions.Add();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing user custom action");
            }

            MapCustomAction(existingAction, model);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingAction,
                ObjectType = typeof(UserCustomAction),
                ObjectDefinition = model,
                ModelHost = modelHost
            });


            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling existingAction.Update()");
            existingAction.Update();

            context.ExecuteQueryWithTrace();
        }

        private void MapCustomAction(UserCustomAction existringAction, UserCustomActionDefinition customAction)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Updating user custom action properties.");

            existringAction.Sequence = customAction.Sequence;
            existringAction.Description = customAction.Description;
            existringAction.Group = customAction.Group;
            existringAction.Location = customAction.Location;
            existringAction.Name = customAction.Name;
            existringAction.ScriptBlock = customAction.ScriptBlock;
            existringAction.ScriptSrc = customAction.ScriptSrc;
            existringAction.Title = customAction.Title;
            existringAction.Url = customAction.Url;

            if (!string.IsNullOrEmpty(customAction.RegistrationId))
                existringAction.RegistrationId = customAction.RegistrationId;

            if (!string.IsNullOrEmpty(customAction.RegistrationType))
                existringAction.RegistrationType = (UserCustomActionRegistrationType)Enum.Parse(typeof(UserCustomActionRegistrationType), customAction.RegistrationType, true);

            var permissions = new BasePermissions();

            if (customAction.Rights != null && customAction.Rights.Count > 0)
            {
                foreach (var permissionString in customAction.Rights)
                    permissions.Set((PermissionKind)Enum.Parse(typeof(PermissionKind), permissionString));
            }

            existringAction.Rights = permissions;
        }

        protected bool IsValidHostModelHost(object modelHost)
        {
            return modelHost is SiteModelHost;
        }

        #endregion
    }
}
