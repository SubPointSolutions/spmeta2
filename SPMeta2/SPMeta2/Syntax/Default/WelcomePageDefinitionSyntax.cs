using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class WelcomePageModelNode : TypedModelNode
    {

    }

    public static class WelcomePageDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWelcomePage<TModelNode>(this TModelNode model, WelcomePageDefinition definition)
            where TModelNode : ModelNode, IWelcomePageHostModelNode, new()
        {
            return AddWelcomePage(model, definition, null);
        }

        public static TModelNode AddWelcomePage<TModelNode>(this TModelNode model, WelcomePageDefinition definition,
            Action<WelcomePageModelNode> action)
            where TModelNode : ModelNode, IWelcomePageHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
