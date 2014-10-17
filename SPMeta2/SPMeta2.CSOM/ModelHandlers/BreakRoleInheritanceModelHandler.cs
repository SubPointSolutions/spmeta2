using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    internal static class SecurableHelper
    {
        public static SecurableObject ExtractSecurableObject(object modelHost)
        {
            if (modelHost is SecurableObject)
                return modelHost as SecurableObject;

            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList;

            //if (modelHost is FolderModelHost)
            //    return (modelHost as FolderModelHost).CurrentLibraryFolder.ite;

            //if (modelHost is WebpartPageModelHost)
            //    return (modelHost as WebpartPageModelHost).PageListItem;

            throw new SPMeta2NotImplementedException(string.Format("Model host of type:[{0}] is not supported by SecurityGroupLinkModelHandler yet.",
                modelHost.GetType()));
        }
    }

    public class BreakRoleInheritanceModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(BreakRoleInheritanceDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var breakRoleInheritanceModel = model.WithAssertAndCast<BreakRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            ProcessRoleInheritance(modelHost, securableObject, breakRoleInheritanceModel);
        }

        private void ProcessRoleInheritance(object modelHost, SecurableObject securableObject, BreakRoleInheritanceDefinition breakRoleInheritanceModel)
        {
            var context = securableObject.Context;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = securableObject,
                ObjectType = typeof(SecurableObject),
                ObjectDefinition = breakRoleInheritanceModel,
                ModelHost = modelHost
            });

            if (!securableObject.IsObjectPropertyInstantiated("HasUniqueRoleAssignments"))
            {
                context.Load(securableObject, s => s.HasUniqueRoleAssignments);
                context.ExecuteQuery();
            }

            if (!securableObject.HasUniqueRoleAssignments)
                securableObject.BreakRoleInheritance(breakRoleInheritanceModel.CopyRoleAssignments, breakRoleInheritanceModel.ClearSubscopes);

            if (breakRoleInheritanceModel.ForceClearSubscopes)
            {
                context.Load(securableObject.RoleAssignments);
                context.ExecuteQuery();

                while (securableObject.RoleAssignments.Count > 0)
                    securableObject.RoleAssignments[0].DeleteObject();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = securableObject,
                ObjectType = typeof(SecurableObject),
                ObjectDefinition = breakRoleInheritanceModel,
                ModelHost = modelHost
            });
        }

        protected SecurableObject ExtractSecurableObject(object modelHost)
        {
            return SecurableHelper.ExtractSecurableObject(modelHost);
        }

        #endregion

    }
}
