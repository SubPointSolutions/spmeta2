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

        public static FarmModelNode AddManagedAccount(this FarmModelNode siteModel, ManagedAccountDefinition definition)
        {
            return AddManagedAccount(siteModel, definition, null);
        }

        public static FarmModelNode AddManagedAccount(this FarmModelNode model, ManagedAccountDefinition fielDefinition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(fielDefinition, action);
        }

        #endregion

    }
}
