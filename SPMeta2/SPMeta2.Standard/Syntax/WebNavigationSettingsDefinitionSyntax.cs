using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class WebNavigationSettingsModelNode : TypedModelNode
    {

    }

    public static class WebNavigationSettingsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWebNavigationSettings<TModelNode>(this TModelNode model, WebNavigationSettingsDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddWebNavigationSettings(model, definition, null);
        }

        public static TModelNode AddWebNavigationSettings<TModelNode>(this TModelNode model, WebNavigationSettingsDefinition definition,
            Action<ModuleFileModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
