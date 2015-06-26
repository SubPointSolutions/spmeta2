using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class TreeViewSettingsDefinitionSyntax
    {
        #region methods

        public static WebModelNode AddTreeViewSettings(this WebModelNode model, TreeViewSettingsDefinition definition)
        {
            return AddTreeViewSettings(model, definition, null);
        }

        public static WebModelNode AddTreeViewSettings(this WebModelNode model, TreeViewSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
