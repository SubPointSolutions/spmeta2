using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class DateTimeFieldModelNode : FieldModelNode
    {

    }

    public static class DateTimeFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDateTimeField<TModelNode>(this TModelNode model, DateTimeFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddDateTimeField(model, definition, null);
        }

        public static TModelNode AddDateTimeField<TModelNode>(this TModelNode model, DateTimeFieldDefinition definition,
            Action<DateTimeFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddDateTimeFields<TModelNode>(this TModelNode model, IEnumerable<DateTimeFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
