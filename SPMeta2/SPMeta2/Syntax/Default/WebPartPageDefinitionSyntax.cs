using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class WebPartPageModelNode : TypedModelNode
    {

    }

    public static class WebPartPageDefinitionSyntax
    {
        #region methods

        public static ListModelNode AddWebPartPage(this ListModelNode model, WebPartPageDefinition definition)
        {
            return AddWebPartPage(model, definition, null);
        }

        public static ListModelNode AddWebPartPage(this ListModelNode model, WebPartPageDefinition definition, Action<WebPartPageModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddWebPartPages(this ModelNode model, IEnumerable<WebPartPageDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override

        public static ListModelNode AddHostWebPartPage(this ListModelNode model, WebPartPageDefinition definition)
        {
            return AddHostWebPartPage(model, definition, null);
        }

        public static ListModelNode AddHostWebPartPage(this ListModelNode model, WebPartPageDefinition definition,
            Action<WebPartPageModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
