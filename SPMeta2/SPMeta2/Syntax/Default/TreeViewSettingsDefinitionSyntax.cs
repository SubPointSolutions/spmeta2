using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class TreeViewSettingsModelNode : TypedModelNode
    {

    }

    public static class TreeViewSettingsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTreeViewSettings<TModelNode>(this TModelNode model, TreeViewSettingsDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddTreeViewSettings(model, definition, null);
        }

        public static TModelNode AddTreeViewSettings<TModelNode>(this TModelNode model, TreeViewSettingsDefinition definition,
            Action<TreeViewSettingsModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
