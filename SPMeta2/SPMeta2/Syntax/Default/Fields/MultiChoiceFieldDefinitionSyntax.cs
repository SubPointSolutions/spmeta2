using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class MultiChoiceFieldModelNode : FieldModelNode
    {

    }

    public static class MultiChoiceFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddMultiChoiceField<TModelNode>(this TModelNode model, MultiChoiceFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddMultiChoiceField(model, definition, null);
        }

        public static TModelNode AddMultiChoiceField<TModelNode>(this TModelNode model, MultiChoiceFieldDefinition definition,
            Action<MultiChoiceFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddMultiChoiceFields<TModelNode>(this TModelNode model, IEnumerable<MultiChoiceFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
