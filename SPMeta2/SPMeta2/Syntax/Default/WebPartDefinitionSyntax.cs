using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class WebPartDefinitionSyntax
    {
        #region methods


        public static ModelNode AddWebPart(this ModelNode model, WebPartDefinition definition)
        {
            return AddWebPart(model, definition, null);
        }

        public static ModelNode AddWebPart(this ModelNode model, WebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddWebParts(this ModelNode model, IEnumerable<WebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
