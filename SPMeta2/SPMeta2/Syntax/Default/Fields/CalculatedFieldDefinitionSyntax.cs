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
    public class CalculatedFieldModelNode : FieldModelNode
    {

    }

    public static class CalculatedFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddCalculatedField<TModelNode>(this TModelNode model, CalculatedFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddCalculatedField(model, definition, null);
        }

        public static TModelNode AddCalculatedField<TModelNode>(this TModelNode model, CalculatedFieldDefinition definition,
            Action<CalculatedFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddCalculatedFields<TModelNode>(this TModelNode model, IEnumerable<CalculatedFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
