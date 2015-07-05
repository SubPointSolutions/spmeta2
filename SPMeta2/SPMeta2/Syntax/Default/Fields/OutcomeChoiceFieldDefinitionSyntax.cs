using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class OutcomeChoiceFieldModelNode : FieldModelNode
    {

    }

    public static class OutcomeChoiceFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddOutcomeChoiceField<TModelNode>(this TModelNode model, OutcomeChoiceFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddOutcomeChoiceField(model, definition, null);
        }

        public static TModelNode AddOutcomeChoiceField<TModelNode>(this TModelNode model, OutcomeChoiceFieldDefinition definition,
            Action<OutcomeChoiceFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddOutcomeChoiceFields<TModelNode>(this TModelNode model, IEnumerable<OutcomeChoiceFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
