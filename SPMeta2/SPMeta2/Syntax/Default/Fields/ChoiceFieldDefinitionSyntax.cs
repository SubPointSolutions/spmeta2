using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class ChoiceFieldModelNode : FieldModelNode
    {

    }

    public static class ChoiceFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddChoiceField<TModelNode>(this TModelNode model, ChoiceFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddChoiceField(model, definition, null);
        }

        public static TModelNode AddChoiceField<TModelNode>(this TModelNode model, ChoiceFieldDefinition definition,
            Action<ChoiceFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddChoiceFields<TModelNode>(this TModelNode model, IEnumerable<ChoiceFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
