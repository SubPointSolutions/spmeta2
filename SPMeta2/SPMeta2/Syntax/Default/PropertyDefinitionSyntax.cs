using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class PropertyModelNode : TypedModelNode
    {

    }

    public static class PropertyDefinitionSyntax
    {

        #region methods

        public static TModelNode AddProperty<TModelNode>(this TModelNode model, PropertyDefinition definition)
            where TModelNode : ModelNode, IPropertyHostModelNode, new()
        {
            return AddProperty(model, definition, null);
        }

        public static TModelNode AddProperty<TModelNode>(this TModelNode model, PropertyDefinition definition,
            Action<PropertyModelNode> action)
            where TModelNode : ModelNode, IPropertyHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddProperties<TModelNode>(this TModelNode model, IEnumerable<PropertyDefinition> definitions)
           where TModelNode : ModelNode, IPropertyHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}