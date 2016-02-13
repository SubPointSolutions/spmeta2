using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class BooleanFieldModelNode : FieldModelNode
    {

    }

    public static class BooleanFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddBooleanField<TModelNode>(this TModelNode model, BooleanFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddBooleanField(model, definition, null);
        }

        public static TModelNode AddBooleanField<TModelNode>(this TModelNode model, BooleanFieldDefinition definition,
            Action<BooleanFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddBooleanFields<TModelNode>(this TModelNode model, IEnumerable<BooleanFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
