using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SecurityGroupModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SecurityGroupDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostSite.RootWeb;
            var securityGroupModel = model as SecurityGroupDefinition;

            if (web != null && securityGroupModel != null)
            {
                var context = web.Context;

                context.Load(web, tmpWeb => tmpWeb.SiteGroups);
                context.ExecuteQueryWithTrace();

                var currentGroup = FindSecurityGroupByTitle(web.SiteGroups, securityGroupModel.Name);

                //action(new ModelHostContext
                action(new SecurityGroupModelHost
                {
                    SecurableObject = web,
                    SecurityGroup = currentGroup
                });

                currentGroup.Update();
                context.ExecuteQueryWithTrace();
            }
            else
            {
                action(modelHost);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostSite.RootWeb;
            var securityGroupModel = model.WithAssertAndCast<SecurityGroupDefinition>("model", value => value.RequireNotNull());

            var context = web.Context;

            // well, this should be pulled up to the site handler and init Load/Exec query
            context.Load(web, tmpWeb => tmpWeb.SiteGroups, g => g.Id, g => g.Title);
            context.ExecuteQueryWithTrace();

            Principal groupOwner = null;

            if (!string.IsNullOrEmpty(securityGroupModel.Owner) &&
                (securityGroupModel.Owner.ToUpper() != securityGroupModel.Name.ToUpper()))
            {
                groupOwner = ResolvePrincipal(context, web, securityGroupModel.Owner);

                if (groupOwner == null)
                    throw new SPMeta2Exception(string.Format("Cannot resolve Principal by string value: [{0}]", securityGroupModel.Owner));
            }

            var currentGroup = FindSecurityGroupByTitle(web.SiteGroups, securityGroupModel.Name);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentGroup,
                ObjectType = typeof(Group),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (currentGroup == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new security group");

                currentGroup = web.SiteGroups.Add(new GroupCreationInformation
                {
                    Title = securityGroupModel.Name,
                    Description = securityGroupModel.Description ?? string.Empty,
                });

                currentGroup.Update();
                context.ExecuteQueryWithTrace();

                // updating the owner or leave as default
                // Enhance 'SecurityGroupDefinition' provision - add self-owner support #516
                // https://github.com/SubPointSolutions/spmeta2/issues/516
                if (!string.IsNullOrEmpty(securityGroupModel.Owner))
                {
                    groupOwner = ResolvePrincipal(context, web, securityGroupModel.Owner);
                    currentGroup.Owner = groupOwner;

                    currentGroup.Update();
                    context.ExecuteQueryWithTrace();
                }

                context.Load(currentGroup);
                context.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing security group");
            }

            currentGroup.Title = securityGroupModel.Name;
            currentGroup.Description = securityGroupModel.Description ?? string.Empty;
            currentGroup.OnlyAllowMembersViewMembership = securityGroupModel.OnlyAllowMembersViewMembership;

            if (securityGroupModel.AllowMembersEditMembership.HasValue)
                currentGroup.AllowMembersEditMembership = securityGroupModel.AllowMembersEditMembership.Value;

            if (securityGroupModel.AllowRequestToJoinLeave.HasValue)
                currentGroup.AllowRequestToJoinLeave = securityGroupModel.AllowRequestToJoinLeave.Value;

            if (securityGroupModel.AutoAcceptRequestToJoinLeave.HasValue)
                currentGroup.AutoAcceptRequestToJoinLeave = securityGroupModel.AutoAcceptRequestToJoinLeave.Value;

            if (groupOwner != null)
                currentGroup.Owner = groupOwner;


            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentGroup,
                ObjectType = typeof(Group),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            currentGroup.Update();
            context.ExecuteQueryWithTrace();
        }

        private Principal ResolvePrincipal(ClientRuntimeContext context, Web web, string owner)
        {
            Principal result = null;

            var targetSources = new Dictionary<PrincipalType, PrincipalInfo>();

            // owner might be only a user or sharepoint group
            // making a few attempts and checking NULL ref later in the code
            targetSources.Add(PrincipalType.SharePointGroup, null);
            targetSources.Add(PrincipalType.User, null);

            foreach (var targetSource in targetSources.Keys)
            {
                // ResolvePrincipal != SearchPrincipals, at all!

                //var principalInfos = Utility.ResolvePrincipal(context, web, owner, targetSource, PrincipalSource.All, null, false);
                var principalInfos = Utility.SearchPrincipals(context, web, owner, targetSource, PrincipalSource.All, null, 2);
                context.ExecuteQuery();

                if (principalInfos.Count > 0)
                //if (principalInfos.Value != null)
                {
                    var info = principalInfos[0];
                    //var info = principalInfos.Value;

                    targetSources[targetSource] = info;

                    if (targetSource == PrincipalType.User || targetSource == PrincipalType.SecurityGroup)
                        result = web.EnsureUser(info.LoginName);

                    if (targetSource == PrincipalType.SharePointGroup)
                        result = web.SiteGroups.GetById(info.PrincipalId);

                    context.Load(result);
                    context.ExecuteQuery();

                    // nic, found, break, profit!
                    break;
                }
            }

            return result;
        }

        protected Group FindSecurityGroupByTitle(IEnumerable<Group> siteGroups, string securityGroupTitle)
        {
            // gosh, who cares ab GetById() methods?! Where GetByName()?!

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving security group by title: [{0}]", securityGroupTitle);

            foreach (var securityGroup in siteGroups)
            {
                if (System.String.Compare(securityGroup.Title, securityGroupTitle, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return securityGroup;
            }

            return null;
        }

        #endregion
    }
}
