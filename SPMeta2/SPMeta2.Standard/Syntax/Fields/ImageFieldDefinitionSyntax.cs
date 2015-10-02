using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class ImageFieldModelNode : FieldModelNode
    {

    }


    public static class ImageFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddImageField<TModelNode>(this TModelNode model, ImageFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddImageField(model, definition, null);
        }

        public static TModelNode AddImageField<TModelNode>(this TModelNode model, ImageFieldDefinition definition,
            Action<ImageFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddImageFields<TModelNode>(this TModelNode model, IEnumerable<ImageFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
