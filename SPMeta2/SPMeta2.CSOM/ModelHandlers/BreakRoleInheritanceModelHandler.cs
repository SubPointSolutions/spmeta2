using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
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

            if (modelHost is FolderModelHost)
            {
                var folderModelHost = modelHost as FolderModelHost;

                if (folderModelHost.CurrentLibraryFolder != null)
                    return folderModelHost.CurrentLibraryFolder.ListItemAllFields;
                else
                    return folderModelHost.CurrentListItem;
            }

            if (modelHost is ListItemModelHost)
            {
                var listItemModelHost = modelHost as ListItemModelHost;

                return listItemModelHost.HostListItem;
            }

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
                context.ExecuteQueryWithTrace();
            }

            if (!securableObject.HasUniqueRoleAssignments)
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                    "HasUniqueRoleAssignments is FALSE. Breaking role inheritance with CopyRoleAssignments: [{0}] and ClearSubscopes: [{1}]",
                    new object[]
                    {
                        breakRoleInheritanceModel.CopyRoleAssignments,
                        breakRoleInheritanceModel.ClearSubscopes
                    });

                securableObject.BreakRoleInheritance(breakRoleInheritanceModel.CopyRoleAssignments, breakRoleInheritanceModel.ClearSubscopes);
            }

            if (breakRoleInheritanceModel.ForceClearSubscopes)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ForceClearSubscopes is TRUE. Removing all role assignments.");

                context.Load(securableObject.RoleAssignments);
                context.ExecuteQueryWithTrace();

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
