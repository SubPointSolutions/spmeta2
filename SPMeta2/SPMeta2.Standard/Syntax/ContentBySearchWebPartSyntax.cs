using System;
using System.Collections.Generic;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class ContentBySearchWebPartSyntax
    {
        #region methods

        public static ModelNode AddContentBySearchWebPart(this ModelNode model, ContentBySearchWebPartDefinition definition)
        {
            return AddContentBySearchWebPart(model, definition, null);
        }

        public static ModelNode AddContentBySearchWebPart(this ModelNode model, ContentBySearchWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddContentBySearchWebParts(this ModelNode model, IEnumerable<ContentBySearchWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
