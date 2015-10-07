using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class GeolocationFieldModelNode : FieldModelNode
    {

    }

    public static class GeolocationFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddGeolocationField<TModelNode>(this TModelNode model, GeolocationFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddGeolocationField(model, definition, null);
        }

        public static TModelNode AddGeolocationField<TModelNode>(this TModelNode model, GeolocationFieldDefinition definition,
            Action<GeolocationFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddGeolocationFields<TModelNode>(this TModelNode model, IEnumerable<GeolocationFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
