using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class ClearRecycleBinModelNode : TypedModelNode
    {

    }

    public static class ClearRecycleBinDefinitionSyntax
    {
        #region methods

        public static TModelNode AddClearRecycleBin<TModelNode>(this TModelNode model, ClearRecycleBinDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddClearRecycleBin(model, definition, null);
        }

        public static TModelNode AddClearRecycleBin<TModelNode>(this TModelNode model, ClearRecycleBinDefinition definition,
            Action<ClearRecycleBinModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddClearRecycleBins<TModelNode>(this TModelNode model, IEnumerable<ClearRecycleBinDefinition> definitions)
           where TModelNode : ModelNode, IWebModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
