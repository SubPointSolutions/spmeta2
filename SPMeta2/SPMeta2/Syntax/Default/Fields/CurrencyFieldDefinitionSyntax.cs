using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class CurrencyFieldModelNode : FieldModelNode
    {

    }

    public static class CurrencyFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddCurrencyField<TModelNode>(this TModelNode model, CurrencyFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddCurrencyField(model, definition, null);
        }

        public static TModelNode AddCurrencyField<TModelNode>(this TModelNode model, CurrencyFieldDefinition definition,
            Action<CurrencyFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddCurrencyFields<TModelNode>(this TModelNode model, IEnumerable<CurrencyFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
