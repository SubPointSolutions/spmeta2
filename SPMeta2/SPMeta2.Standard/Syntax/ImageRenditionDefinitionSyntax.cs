using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class ImageRenditionModelNode : ListItemModelNode
    {

    }

    public static class ImageRenditionDefinitionSyntax
    {
        #region publishing page

        public static SiteModelNode AddImageRendition(this SiteModelNode model, ImageRenditionDefinition definition)
        {
            return AddImageRendition(model, definition, null);
        }

        public static SiteModelNode AddImageRendition(this SiteModelNode model, ImageRenditionDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static SiteModelNode AddImageRenditions(this SiteModelNode model, IEnumerable<ImageRenditionDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
