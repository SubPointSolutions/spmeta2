using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class PrefixModelNode : TypedModelNode
    {

    }

    public static class PrefixDefinitionSyntax
    {
        #region methods

        public static TModelNode AddPrefix<TModelNode>(this TModelNode model, PrefixDefinition definition)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return AddPrefix(model, definition, null);
        }

        public static TModelNode AddPrefix<TModelNode>(this TModelNode model, PrefixDefinition definition,
            Action<PrefixModelNode> action)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddPrefixs<TModelNode>(this TModelNode model, IEnumerable<PrefixDefinition> definitions)
           where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
