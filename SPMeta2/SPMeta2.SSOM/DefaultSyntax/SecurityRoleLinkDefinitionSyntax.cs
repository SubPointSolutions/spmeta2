using Microsoft.SharePoint;
using SPMeta2.Models;
using System;

namespace SPMeta2.SSOM.DefaultSyntax
{
    [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
    public static class SecurityRoleLinkDefinitionSyntax
    {
        #region methods

        //public static DefinitionBase OnCreated(this DefinitionBase model, Action<SecurityRoleLinkDefinition, SPRoleDefinition> action)
        //{
        //    model.RegisterModelUpdatedEvent(action);

        //    return model;
        //}

        //public static DefinitionBase OnCreating(this DefinitionBase model, Action<SecurityRoleLinkDefinition, SPRoleDefinition> action)
        //{
        //    model.RegisterModelUpdatedEvent(action);

        //    return model;
        //}

        //public static ModelNode AddSecurityRoleLink(this  ModelNode model, SPRoleDefinition securityRoleDefinition)
        //{
        //    return model.AddSecurityRoleLink(securityRoleDefinition);
        //}

        #endregion
    }
}
