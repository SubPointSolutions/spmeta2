using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class WikiPageModelNode : TypedModelNode
    {

    }

    public static class WikiPageDefinitionSyntax
    {
        #region methods

        public static ListModelNode AddWikiPage(this ListModelNode model, WikiPageDefinition definition)
        {
            return AddWikiPage(model, definition, null);
        }

        public static ListModelNode AddWikiPage(this ListModelNode model, WikiPageDefinition definition, Action<WikiPageModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddWikiPages(this ModelNode model, IEnumerable<WikiPageDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override

        public static ListModelNode AddHostWikiPage(this ListModelNode model, WikiPageDefinition definition)
        {
            return AddHostWikiPage(model, definition, null);
        }

        public static ListModelNode AddHostWikiPage(this ListModelNode model, WikiPageDefinition definition,
            Action<WikiPageModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
