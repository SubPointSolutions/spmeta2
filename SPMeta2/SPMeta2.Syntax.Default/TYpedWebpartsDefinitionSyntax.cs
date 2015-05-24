using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class TypedWebpartsDefinitionSyntax
    {
        #region AddPageViewerWebPart

        public static ModelNode AddPageViewerWebPart(this ModelNode model, PageViewerWebPartDefinition definition)
        {
            return AddPageViewerWebPart(model, definition, null);
        }

        public static ModelNode AddPageViewerWebPart(this ModelNode model, PageViewerWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddPageViewerWebParts(this ModelNode model, IEnumerable<PageViewerWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

       
    }
}
