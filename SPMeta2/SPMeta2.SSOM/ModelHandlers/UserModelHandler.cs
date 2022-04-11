using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class UserModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(UserDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<UserDefinition>("model", value => value.RequireNotNull());

            DeployArtifact(typedModelHost, typedModelHost.SecurityGroup, definition);
        }

        private void DeployArtifact(SecurityGroupModelHost typedModelHost, SPGroup spGroup, UserDefinition definition)
        {
            var shouldUpdate = false;
            var existingUser = GetUserInGroup(spGroup.ParentWeb, spGroup, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingUser,
                ObjectType = typeof(SPUser),
                ObjectDefinition = definition,
                ModelHost = typedModelHost
            });

            if (existingUser == null)
            {
                var web = spGroup.ParentWeb;
                SPUser targetUser = null;

                if (!string.IsNullOrEmpty(definition.LoginName))
                {
                    targetUser = web.EnsureUser(definition.LoginName);
                }
                else if (!string.IsNullOrEmpty(definition.Email))
                {
                    targetUser = web.EnsureUser(definition.Email);
                }

                spGroup.Users.Add(targetUser.LoginName, targetUser.Email, targetUser.LoginName, targetUser.Notes);
                shouldUpdate = true;

                existingUser = targetUser;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingUser,
                ObjectType = typeof(SPUser),
                ObjectDefinition = definition,
                ModelHost = typedModelHost
            });

            if (shouldUpdate)
            {
                spGroup.Update();
            }
        }

        protected virtual SPUser GetUserInGroup(SPWeb spWeb, SPGroup spGroup, UserDefinition definition)
        {
            SPUser result = null;

            if (!string.IsNullOrEmpty(definition.LoginName))
            {
                result = spGroup.Users.OfType<SPUser>()
                                       .FirstOrDefault(u => u.LoginName.ToUpper() == definition.LoginName.ToUpper());
            }

            if (result == null)
            {
                if (!string.IsNullOrEmpty(definition.Email))
                {
                    result = spGroup.Users.OfType<SPUser>()
                                           .FirstOrDefault(u =>
                                               !string.IsNullOrEmpty(u.Email)
                                               && (u.Email.ToUpper() == definition.Email.ToUpper()));
                }
            }

            return result;
        }

        #endregion
    }
}
