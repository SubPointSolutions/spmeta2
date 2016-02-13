using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    
    [Serializable]
    [DataContract]
    public class GuidFieldModelNode : FieldModelNode
    {

    }

    public static class GuidFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddGuidField<TModelNode>(this TModelNode model, GuidFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddGuidField(model, definition, null);
        }

        public static TModelNode AddGuidField<TModelNode>(this TModelNode model, GuidFieldDefinition definition,
            Action<GuidFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddGuidFields<TModelNode>(this TModelNode model, IEnumerable<GuidFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
