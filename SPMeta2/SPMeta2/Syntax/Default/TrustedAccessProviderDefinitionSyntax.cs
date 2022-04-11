using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class TrustedAccessProviderModelNode : TypedModelNode
    {

    }

    public static class TrustedAccessProviderDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTrustedAccessProvider<TModelNode>(this TModelNode model, TrustedAccessProviderDefinition definition)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return AddTrustedAccessProvider(model, definition, null);
        }

        public static TModelNode AddTrustedAccessProvider<TModelNode>(this TModelNode model, TrustedAccessProviderDefinition definition,
            Action<TrustedAccessProviderModelNode> action)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddTrustedAccessProviders<TModelNode>(this TModelNode model, IEnumerable<TrustedAccessProviderDefinition> definitions)
           where TModelNode : ModelNode, IFarmModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
