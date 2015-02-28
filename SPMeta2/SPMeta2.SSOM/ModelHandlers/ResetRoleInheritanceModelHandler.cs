using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ResetRoleInheritanceModelHandler : SSOMModelHandlerBase
    {
        protected SPSecurableObject ExtractSecurableObject(object modelHost)
        {
            return SecurableHelper.ExtractSecurableObject(modelHost);
        }

        public override Type TargetType
        {
            get { return typeof(ResetRoleInheritanceDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var breakRoleInheritanceModel = model.WithAssertAndCast<ResetRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            ProcessRoleInheritance(modelHost, securableObject, breakRoleInheritanceModel);
        }

        private void ProcessRoleInheritance(object modelHost, SPSecurableObject securableObject, ResetRoleInheritanceDefinition breakRoleInheritanceModel)
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

            if (securableObject.HasUniqueRoleAssignments)
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "HasUniqueRoleAssignments is TRUE. Resetting role inheritance", null);
                securableObject.ResetRoleInheritance();
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
    }
}
