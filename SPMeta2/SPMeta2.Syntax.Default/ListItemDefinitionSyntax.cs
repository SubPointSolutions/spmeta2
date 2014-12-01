using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ListItemDefinitionSyntax
    {
        #region methods

        public static ModelNode AddListItem(this ModelNode model, ListItemDefinition definition)
        {
            return AddListItem(model, definition, null);
        }

        public static ModelNode AddListItem(this ModelNode model, ListItemDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region add host

        public static ModelNode AddHostListItem(this ModelNode model, ListItemDefinition definition)
        {
            return AddHostListItem(model, definition, null);
        }

        public static ModelNode AddHostListItem(this ModelNode model, ListItemDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
