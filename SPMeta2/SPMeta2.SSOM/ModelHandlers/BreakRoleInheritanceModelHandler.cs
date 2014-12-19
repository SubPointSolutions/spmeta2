using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class BreakRoleInheritanceModelHandler : SSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(BreakRoleInheritanceDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var breakRoleInheritanceModel = model.WithAssertAndCast<BreakRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            ProcessRoleInheritance(modelHost, securableObject, breakRoleInheritanceModel);
        }

        private void ProcessRoleInheritance(object modelHost, SPSecurableObject securableObject, BreakRoleInheritanceDefinition breakRoleInheritanceModel)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = securableObject,
                ObjectType = typeof(SPSecurableObject),
                ObjectDefinition = breakRoleInheritanceModel,
                ModelHost = modelHost
            });

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

                while (securableObject.RoleAssignments.Count > 0)
                    securableObject.RoleAssignments.Remove(0);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = securableObject,
                ObjectType = typeof(SPSecurableObject),
                ObjectDefinition = breakRoleInheritanceModel,
                ModelHost = modelHost
            });
        }

        protected SPSecurableObject ExtractSecurableObject(object modelHost)
        {
            return SecurableHelper.ExtractSecurableObject(modelHost);
        }
    }
}
