using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class BusinessDataFieldModelNode : FieldModelNode
    {

    }

    public static class BusinessDataFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddBusinessDataField<TModelNode>(this TModelNode model, BusinessDataFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddBusinessDataField(model, definition, null);
        }

        public static TModelNode AddBusinessDataField<TModelNode>(this TModelNode model, BusinessDataFieldDefinition definition,
            Action<BusinessDataFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddBusinessDataFields<TModelNode>(this TModelNode model, IEnumerable<BusinessDataFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
