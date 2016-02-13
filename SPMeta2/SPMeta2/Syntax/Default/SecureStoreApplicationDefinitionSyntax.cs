using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SecureStoreApplicationModelNode : TypedModelNode,
        ITargetApplicationHostModelNode
    {

    }

    public static class SecureStoreApplicationDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSecureStoreApplication<TModelNode>(this TModelNode model, SecureStoreApplicationDefinition definition)
            where TModelNode : ModelNode, ISecureStoreApplicationHostModelNode, new()
        {
            return AddSecureStoreApplication(model, definition, null);
        }

        public static TModelNode AddSecureStoreApplication<TModelNode>(this TModelNode model, SecureStoreApplicationDefinition definition,
            Action<SecureStoreApplicationModelNode> action)
            where TModelNode : ModelNode, ISecureStoreApplicationHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region add host

        public static TModelNode AddHostSecureStoreApplication<TModelNode>(this TModelNode model, SecureStoreApplicationDefinition definition)
             where TModelNode : ModelNode, ISecureStoreApplicationHostModelNode, new()
        {
            return AddHostSecureStoreApplication(model, definition, null);
        }
        public static TModelNode AddHostSecureStoreApplication<TModelNode>(this TModelNode model, SecureStoreApplicationDefinition definition,
            Action<SecureStoreApplicationModelNode> action)
            where TModelNode : ModelNode, ISecureStoreApplicationHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
