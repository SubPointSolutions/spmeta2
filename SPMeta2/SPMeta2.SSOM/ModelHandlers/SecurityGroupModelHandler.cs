using System;
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

                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing security group");

            }
            catch (SPException)
            {
                var defaultUser = EnsureDefaultUser(web, securityGroupModel);

                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new security group");

                // owner would be defaut site owner
                web.SiteGroups.Add(securityGroupModel.Name, web.Site.Owner, defaultUser, securityGroupModel.Description ?? string.Empty);
                currentGroup = web.SiteGroups[securityGroupModel.Name];

                // updating the owner or leave as default
                // Enhance 'SecurityGroupDefinition' provision - add self-owner support #516
                // https://github.com/SubPointSolutions/spmeta2/issues/516
                var ownerUser = EnsureOwnerUser(web, securityGroupModel.Owner);

                currentGroup.Owner = ownerUser;
                currentGroup.Update();
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

            currentGroup.OnlyAllowMembersViewMembership = securityGroupModel.OnlyAllowMembersViewMembership;

            if (!string.IsNullOrEmpty(securityGroupModel.Owner))
                currentGroup.Owner = EnsureOwnerUser(web, securityGroupModel.Owner);

            currentGroup.Description = securityGroupModel.Description ?? string.Empty;

            if (securityGroupModel.AllowMembersEditMembership.HasValue)
                currentGroup.AllowMembersEditMembership = securityGroupModel.AllowMembersEditMembership.Value;

            if (securityGroupModel.AllowRequestToJoinLeave.HasValue)
                currentGroup.AllowRequestToJoinLeave = securityGroupModel.AllowRequestToJoinLeave.Value;

            if (securityGroupModel.AutoAcceptRequestToJoinLeave.HasValue)
                currentGroup.AutoAcceptRequestToJoinLeave = securityGroupModel.AutoAcceptRequestToJoinLeave.Value;

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

        protected virtual SPPrincipal EnsureOwnerUser(SPWeb web, string owner)
        {
            if (string.IsNullOrEmpty(owner))
            {
                return web.Site.Owner;
            }
            else
            {
                bool max;

                var principalInfos = SPUtility.SearchPrincipals(web, owner, SPPrincipalType.All, SPPrincipalSource.All, null, 2, out max);

                if (principalInfos.Count > 0)
                //if (principalInfos.Value != null)
                {
                    var info = principalInfos[0];

                    if (info.PrincipalType == SPPrincipalType.User || info.PrincipalType == SPPrincipalType.SecurityGroup)
                        return web.EnsureUser(info.LoginName);

                    if (info.PrincipalType == SPPrincipalType.SharePointGroup)
                        return web.SiteGroups.GetByID(info.PrincipalId);
                }

                throw new SPMeta2Exception(string.Format("Cannot resolve Principal by string value: [{0}]", owner));
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
