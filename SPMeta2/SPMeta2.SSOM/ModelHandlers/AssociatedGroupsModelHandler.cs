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
    public class AssociatedGroupsModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(AssociatedGroupsDefinition); }
        }


        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var groupsDefinition = modelHost.WithAssertAndCast<AssociatedGroupsDefinition>("modelHost", value => value.RequireNotNull());

            DeployAssociatedGroups(modelHost, webModelHost, groupsDefinition);
        }

        protected virtual SPGroup FindGroupByName(SPSite site, SPWeb web, string groupName)
        {
            return web.SiteGroups[groupName];
        }

        protected virtual void DeployAssociatedGroups(object modelHost, WebModelHost webModelHost, AssociatedGroupsDefinition model)
        {
            var web = webModelHost.HostWeb;
            var site = web.Site;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(model.MemberGroupName))
                web.AssociatedMemberGroup = FindGroupByName(site, web, model.MemberGroupName);

            if (!string.IsNullOrEmpty(model.OwnerGroupName))
                web.AssociatedOwnerGroup = FindGroupByName(site, web, model.OwnerGroupName);

            if (!string.IsNullOrEmpty(model.VisitorGroupName))
                web.AssociatedVisitorGroup = FindGroupByName(site, web, model.VisitorGroupName);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = model,
                ModelHost = modelHost
            });


            web.Update();
        }

        #endregion
    }
}
