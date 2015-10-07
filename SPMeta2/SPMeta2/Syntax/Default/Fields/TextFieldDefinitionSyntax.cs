using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class TextFieldModelNode : FieldModelNode
    {

    }

    public static class TextFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTextField<TModelNode>(this TModelNode model, TextFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddTextField(model, definition, null);
        }

        public static TModelNode AddTextField<TModelNode>(this TModelNode model, TextFieldDefinition definition,
            Action<TextFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddTextFields<TModelNode>(this TModelNode model, IEnumerable<TextFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
