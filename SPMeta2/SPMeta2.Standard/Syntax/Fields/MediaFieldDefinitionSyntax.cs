using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class MediaFieldModelNode : FieldModelNode
    {

    }

    public static class MediaFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddMediaField<TModelNode>(this TModelNode model, MediaFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddMediaField(model, definition, null);
        }

        public static TModelNode AddMediaField<TModelNode>(this TModelNode model, MediaFieldDefinition definition,
            Action<FieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddMediaFields<TModelNode>(this TModelNode model, IEnumerable<MediaFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
