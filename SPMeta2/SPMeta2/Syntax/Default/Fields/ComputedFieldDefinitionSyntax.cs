using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class ComputedFieldModelNode : FieldModelNode
    {

    }

    public static class ComputedFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddComputedField<TModelNode>(this TModelNode model, ComputedFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddComputedField(model, definition, null);
        }

        public static TModelNode AddComputedField<TModelNode>(this TModelNode model, ComputedFieldDefinition definition,
            Action<ComputedFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddComputedFields<TModelNode>(this TModelNode model, IEnumerable<ComputedFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

    }
}
