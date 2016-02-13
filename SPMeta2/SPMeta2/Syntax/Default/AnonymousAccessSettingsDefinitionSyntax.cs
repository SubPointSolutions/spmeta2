using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class AnonymousAccessSettingsModelNode : TypedModelNode
    {

    }

    public static class AnonymousAccessSettingsDefinitionSyntax
    {

        #region methods

        public static TModelNode AddAnonymousAccessSettings<TModelNode>(this TModelNode model, AnonymousAccessSettingsDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddAnonymousAccessSettings(model, definition, null);
        }

        public static TModelNode AddAnonymousAccessSettings<TModelNode>(this TModelNode model, AnonymousAccessSettingsDefinition definition,
            Action<TModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
