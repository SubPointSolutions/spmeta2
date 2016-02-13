using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class FieldModelNode : TypedModelNode
    {

    }

    public static class FieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddField<TModelNode>(this TModelNode model, FieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddField(model, definition, null);
        }

        public static TModelNode AddField<TModelNode>(this TModelNode model, FieldDefinition definition,
            Action<FieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddFields<TModelNode>(this TModelNode model, IEnumerable<FieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
