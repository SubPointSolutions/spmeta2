using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class NumberFieldModelNode : FieldModelNode
    {

    }

    public static class NumberFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddNumberField<TModelNode>(this TModelNode model, NumberFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddNumberField(model, definition, null);
        }

        public static TModelNode AddNumberField<TModelNode>(this TModelNode model, NumberFieldDefinition definition,
            Action<NumberFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddNumberFields<TModelNode>(this TModelNode model, IEnumerable<NumberFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
