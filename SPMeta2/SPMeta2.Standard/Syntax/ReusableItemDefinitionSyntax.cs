using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class ReusableItemDefinitionSyntax
    {
        #region html

        public static ModelNode AddReusableHTMLItem(this ModelNode model, ReusableHTMLItemDefinition definition)
        {
            return AddReusableHTMLItem(model, definition, null);
        }

        public static ModelNode AddReusableHTMLItem(this ModelNode model, ReusableHTMLItemDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddReusableHTMLItems(this ModelNode model, IEnumerable<ReusableHTMLItemDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region text

        public static ModelNode AddReusableTextItem(this ModelNode model, ReusableTextItemDefinition definition)
        {
            return AddReusableTextItem(model, definition, null);
        }

        public static ModelNode AddReusableTextItem(this ModelNode model, ReusableTextItemDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddReusableTextItems(this ModelNode model, IEnumerable<ReusableTextItemDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
