using System;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
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
            Action<WebNavigationSettingsModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
