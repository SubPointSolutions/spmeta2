using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ManagedAccountDefinitionSyntax
    {
        #region methods

        public static ModelNode AddManagedAccount(this ModelNode siteModel, ManagedAccountDefinition definition)
        {
            return AddManagedAccount(siteModel, definition, null);
        }

        public static ModelNode AddManagedAccount(this ModelNode model, ManagedAccountDefinition fielDefinition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(fielDefinition, action);
        }

        #endregion

    }
}
