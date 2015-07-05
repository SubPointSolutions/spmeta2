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
