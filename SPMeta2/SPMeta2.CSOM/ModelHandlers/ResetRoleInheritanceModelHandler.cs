using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ResetRoleInheritanceModelHandler : CSOMModelHandlerBase
    {
        protected SecurableObject ExtractSecurableObject(object modelHost)
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

        private void ProcessRoleInheritance(object modelHost, SecurableObject securableObject, ResetRoleInheritanceDefinition breakRoleInheritanceModel)
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

            if (securableObject.HasUniqueRoleAssignments)
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "HasUniqueRoleAssignments is TRUE. Resetting role inheritance", null);
                securableObject.ResetRoleInheritance();

                context.ExecuteQueryWithTrace();
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
    }
}
