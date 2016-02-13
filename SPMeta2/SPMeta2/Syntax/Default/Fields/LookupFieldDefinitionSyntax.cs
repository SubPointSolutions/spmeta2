using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class LookupFieldModelNode : FieldModelNode
    {

    }

    public static class LookupFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddLookupField<TModelNode>(this TModelNode model, LookupFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddLookupField(model, definition, null);
        }

        public static TModelNode AddLookupField<TModelNode>(this TModelNode model, LookupFieldDefinition definition,
            Action<LookupFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddLookupFields<TModelNode>(this TModelNode model, IEnumerable<LookupFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
