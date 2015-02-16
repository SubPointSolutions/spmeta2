using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.BusinessData.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class UserCustomActionModelHandler : SSOMModelHandlerBase
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

        protected SPUserCustomAction GetCurrentCustomUserAction(object modelHost,
            UserCustomActionDefinition customActionModel)
        {
            SPUserCustomActionCollection userCustomActions = null;

            return GetCurrentCustomUserAction(modelHost, customActionModel, out userCustomActions);
        }

        private SPUserCustomAction GetCurrentCustomUserAction(object modelHost, UserCustomActionDefinition customActionModel
            , out SPUserCustomActionCollection userCustomActions)
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

            return userCustomActions.FirstOrDefault(a => !string.IsNullOrEmpty(a.Name) && a.Name.ToUpper() == customActionModel.Name.ToUpper());
        }

        private void DeploySiteCustomAction(
            object modelHost,
            UserCustomActionDefinition customActionModel)
        {
            SPUserCustomActionCollection userCustomActions = null;
            var existingAction = GetCurrentCustomUserAction(modelHost, customActionModel, out userCustomActions);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingAction,
                ObjectType = typeof(SPUserCustomAction),
                ObjectDefinition = customActionModel,
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

            MapCustomAction(existingAction, customActionModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingAction,
                ObjectType = typeof(SPUserCustomAction),
                ObjectDefinition = customActionModel,
                ModelHost = modelHost
            });

            existingAction.Update();
        }

        private void MapCustomAction(SPUserCustomAction existringAction, UserCustomActionDefinition customAction)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Updating user custom action properties.");

            existringAction.Description = customAction.Description;
            existringAction.Group = customAction.Group;
            existringAction.Location = customAction.Location;
            existringAction.Name = customAction.Name;
            existringAction.ScriptBlock = customAction.ScriptBlock;
            existringAction.ScriptSrc = customAction.ScriptSrc;
            existringAction.Title = customAction.Title;
            existringAction.Url = customAction.Url;

            existringAction.Sequence = customAction.Sequence;

            if (!string.IsNullOrEmpty(customAction.CommandUIExtension))
                existringAction.CommandUIExtension = customAction.CommandUIExtension;

            if (!string.IsNullOrEmpty(customAction.RegistrationId))
                existringAction.RegistrationId = customAction.RegistrationId;

            if (!string.IsNullOrEmpty(customAction.RegistrationType))
            {
                // skipping setup for List script 
                // System.NotSupportedException: Setting this property is not supported.  A value of List has already been set and cannot be changed.
                if (existringAction.RegistrationType != SPUserCustomActionRegistrationType.List)
                {
                    existringAction.RegistrationType =
                        (SPUserCustomActionRegistrationType)
                            Enum.Parse(typeof(SPUserCustomActionRegistrationType), customAction.RegistrationType, true);
                }
            }

            var permissions = SPBasePermissions.EmptyMask;

            if (customAction.Rights != null && customAction.Rights.Count > 0)
            {
                foreach (var permissionString in customAction.Rights)
                    permissions = permissions | (SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), permissionString);
            }

            existringAction.Rights = permissions;
        }

        private bool IsValidHostModelHost(object modelHost)
        {
            return
                modelHost is SiteModelHost ||
                modelHost is WebModelHost ||
                modelHost is ListModelHost;
        }

        #endregion
    }
}
