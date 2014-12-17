using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
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

            var customAction = model.WithAssertAndCast<UserCustomActionDefinition>("model", value => value.RequireNotNull());

            DeploySiteCustomAction(modelHost, customAction);
        }

        protected UserCustomAction GetCurrentCustomUserAction(object modelHost,
           UserCustomActionDefinition customActionModel)
        {
            UserCustomActionCollection userCustomActions = null;

            return GetCurrentCustomUserAction(modelHost, customActionModel, out userCustomActions);
        }

        private UserCustomAction GetCurrentCustomUserAction(object modelHost, UserCustomActionDefinition customActionModel
            , out UserCustomActionCollection userCustomActions)
        {
            if (modelHost is SiteModelHost)
                userCustomActions = (modelHost as SiteModelHost).HostSite.UserCustomActions;
            else if (modelHost is WebModelHost)
                userCustomActions = (modelHost as WebModelHost).HostWeb.UserCustomActions;
            else if (modelHost is ListModelHost)
                userCustomActions = (modelHost as ListModelHost).HostList.UserCustomActions;
            else
            {
                throw new Exception(string.Format("modelHost of type {0} is not supported.", modelHost.GetType()));
            }

            var context = userCustomActions.Context;

            context.Load(userCustomActions);
            context.ExecuteQueryWithTrace();

            return userCustomActions.FirstOrDefault(a => !string.IsNullOrEmpty(a.Name) && a.Name.ToUpper() == customActionModel.Name.ToUpper());
        }

        private void DeploySiteCustomAction(object modelHost, UserCustomActionDefinition model)
        {
            UserCustomActionCollection userCustomActions = null;
            var existingAction = GetCurrentCustomUserAction(modelHost, model, out userCustomActions);

            var context = userCustomActions.Context;

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
                existingAction = userCustomActions.Add();
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
            {
                // skipping setup for List script 
                // System.NotSupportedException: Setting this property is not supported.  A value of List has already been set and cannot be changed.
                if (customAction.RegistrationType != BuiltInRegistrationTypes.List)
                {
                    existringAction.RegistrationType =
                        (UserCustomActionRegistrationType)
                            Enum.Parse(typeof(UserCustomActionRegistrationType), customAction.RegistrationType, true);
                }
            }

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
            return
                modelHost is SiteModelHost ||
                modelHost is WebModelHost ||
                modelHost is ListModelHost;
        }

        #endregion
    }
}
