using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.Client;

using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class UserModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UserDefinition); }
        }

        #endregion

        #region methods
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<UserDefinition>("model", value => value.RequireNotNull());

            DeployArtifact(typedModelHost, typedModelHost.SecurityGroup, definition);
        }

        private void DeployArtifact(SecurityGroupModelHost typedModelHost, Group spGroup, UserDefinition definition)
        {
            var web = typedModelHost.HostWeb;
            var context = spGroup.Context;

            var shouldUpdate = false;
            var existingUser = GetUserInGroup(web, spGroup, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingUser,
                ObjectType = typeof(User),
                ObjectDefinition = definition,
                ModelHost = typedModelHost
            });

            if (existingUser == null)
            {
                User targetUser = null;

                if (!string.IsNullOrEmpty(definition.LoginName))
                {
                    targetUser = web.EnsureUser(definition.LoginName);
                }
                else if (!string.IsNullOrEmpty(definition.Email))
                {
                    targetUser = web.EnsureUser(definition.Email);
                }

                context.Load(targetUser, u => u.Email,
                                         u => u.LoginName,
                                         u => u.Title);

                context.ExecuteQueryWithTrace();

                spGroup.Users.Add(new UserCreationInformation
                {
                    Email = targetUser.Email,
                    LoginName = targetUser.LoginName,
                    Title = targetUser.Title
                });

                shouldUpdate = true;
                existingUser = targetUser;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingUser,
                ObjectType = typeof(User),
                ObjectDefinition = definition,
                ModelHost = typedModelHost
            });

            if (shouldUpdate)
            {
                spGroup.Update();
                context.ExecuteQueryWithTrace();
            }
        }

        protected virtual User GetUserInGroup(Web spWeb, Group spGroup, UserDefinition definition)
        {
            var context = spGroup.Context;
            User result = null;

            if (!string.IsNullOrEmpty(definition.LoginName))
            {
                try
                {
                    result = spGroup.Users.GetByLoginName(definition.LoginName);
                    context.ExecuteQuery();
                }
                catch
                {
                    result = null;
                }
            }

            if (result == null)
            {
                if (!string.IsNullOrEmpty(definition.Email))
                {
                    try
                    {
                        result = spGroup.Users.GetByEmail(definition.Email);
                        context.ExecuteQuery();
                    }
                    catch
                    {
                        result = null;
                    }
                }
            }

            return result;
        }


        #endregion
    }
}
