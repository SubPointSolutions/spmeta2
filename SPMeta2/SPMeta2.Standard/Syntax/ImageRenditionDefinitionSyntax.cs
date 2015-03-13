using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class ImageRenditionDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddImageRendition(this ModelNode model, ImageRenditionDefinition definition)
        {
            return AddImageRendition(model, definition, null);
        }

        public static ModelNode AddImageRendition(this ModelNode model, ImageRenditionDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddImageRenditions(this ModelNode model, IEnumerable<ImageRenditionDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
