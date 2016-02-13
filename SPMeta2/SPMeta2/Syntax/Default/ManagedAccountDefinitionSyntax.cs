using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ManagedAccountModelNode : TypedModelNode
    {

    }

    public static class ManagedAccountDefinitionSyntax
    {
        #region methods

        public static TModelNode AddManagedAccount<TModelNode>(this TModelNode model, ManagedAccountDefinition definition)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return AddManagedAccount(model, definition, null);
        }

        public static TModelNode AddManagedAccount<TModelNode>(this TModelNode model, ManagedAccountDefinition definition,
            Action<ManagedAccountModelNode> action)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddManagedAccounts<TModelNode>(this TModelNode model, IEnumerable<ManagedAccountDefinition> definitions)
           where TModelNode : ModelNode, IFarmModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
