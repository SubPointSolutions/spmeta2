using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class MediaFieldDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddMediaField(this ModelNode model, MediaFieldDefinition definition)
        {
            return AddMediaField(model, definition, null);
        }

        public static ModelNode AddMediaField(this ModelNode model, MediaFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddMediaFields(this ModelNode model, IEnumerable<MediaFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
