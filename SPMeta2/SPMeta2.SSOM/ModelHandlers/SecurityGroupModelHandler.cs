using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SecurityGroupModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SecurityGroupDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var web = ExtractWeb(modelHost);

            if (web != null)
            {
                string securityGroupName;

                if (model is SecurityGroupLinkDefinition)
                    securityGroupName = (model as SecurityGroupLinkDefinition).SecurityGroupName;
                else if (model is SecurityGroupDefinition)
                    securityGroupName = (model as SecurityGroupDefinition).Name;
                else
                {
                    throw new ArgumentException("model has to be SecurityGroupDefinition or SecurityGroupLinkDefinition");
                }

                var securityGroup = web.SiteGroups[securityGroupName];

                var newModelHost = new SecurityGroupModelHost
                {
                    SecurityGroup = securityGroup,
                    SecurableObject = modelHost as SPSecurableObject
                };

                action(newModelHost);
            }
            else
            {
                action(modelHost);
            }
        }

        private SPWeb ExtractWeb(object modelHost)
        {
            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            if (modelHost is SPWeb)
                return modelHost as SPWeb;

            if (modelHost is SPList)
                return (modelHost as SPList).ParentWeb;

            if (modelHost is SPListItem)
                return (modelHost as SPListItem).ParentList.ParentWeb;

            throw new Exception(string.Format("modelHost with type [{0}] is not supported.", modelHost.GetType()));
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var site = siteModelHost.HostSite;

            var securityGroupModel = model.WithAssertAndCast<SecurityGroupDefinition>("model", value => value.RequireNotNull());

            var web = site.RootWeb;

            //var site = web.Site;
            var currentGroup = (SPGroup)null;
            var hasInitialGroup = false;

            try
            {
                currentGroup = site.RootWeb.SiteGroups[securityGroupModel.Name];
                hasInitialGroup = true;

            }
            catch (SPException)
            {
                var ownerUser = EnsureOwnerUser(web, securityGroupModel);
                var defaultUser = EnsureDefaultUser(web, securityGroupModel);

                web.SiteGroups.Add(securityGroupModel.Name, ownerUser, defaultUser, securityGroupModel.Description);
                currentGroup = web.SiteGroups[securityGroupModel.Name];
            }

            if (hasInitialGroup)
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = currentGroup,
                    ObjectType = typeof(SPGroup),
                    ObjectDefinition = securityGroupModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = null,
                    ObjectType = typeof(SPGroup),
                    ObjectDefinition = securityGroupModel,
                    ModelHost = modelHost
                });
            }

            currentGroup.Owner = EnsureOwnerUser(web, securityGroupModel);
            currentGroup.Description = securityGroupModel.Description;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentGroup,
                ObjectType = typeof(SPGroup),
                ObjectDefinition = securityGroupModel,
                ModelHost = modelHost
            });

            currentGroup.Update();
        }

        protected virtual SPUser EnsureOwnerUser(SPWeb web, SecurityGroupDefinition groupModel)
        {
            if (string.IsNullOrEmpty(groupModel.Owner))
            {
                return web.Site.Owner;
            }
            else
            {
                return web.EnsureUser(groupModel.Owner);
            }
        }

        protected virtual SPUser EnsureDefaultUser(SPWeb web, SecurityGroupDefinition groupModel)
        {
            if (string.IsNullOrEmpty(groupModel.DefaultUser))
            {
                return null;
            }
            else
            {
                return web.EnsureUser(groupModel.DefaultUser);
            }
        }

        #endregion
    }
}
