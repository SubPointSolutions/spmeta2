using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class TargetApplicationModelNode : TypedModelNode
    {

    }

    public static class TargetApplicationDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTargetApplication<TModelNode>(this TModelNode model, TargetApplicationDefinition definition)
            where TModelNode : ModelNode, ITargetApplicationHostModelNode, new()
        {
            return AddTargetApplication(model, definition, null);
        }

        public static TModelNode AddTargetApplication<TModelNode>(this TModelNode model, TargetApplicationDefinition definition,
            Action<TargetApplicationModelNode> action)
            where TModelNode : ModelNode, ITargetApplicationHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
