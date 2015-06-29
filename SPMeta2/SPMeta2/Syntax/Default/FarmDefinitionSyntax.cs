using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class FarmModelNode : TypedModelNode, 
        IPropertyBagHostModelNode,
        IManagedPropertyHostModelNode,
        IFeatureHostModelNode
    {

    }

    public static class FarmDefinitionSyntax
    {
        #region methods

        public static FarmModelNode AddFarm(this FarmModelNode model, FarmDefinition definition)
        {
            return AddFarm(model, definition, null);
        }

        public static FarmModelNode AddFarm(this FarmModelNode model, FarmDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region methods

        public static FarmModelNode AddHostFarm(this FarmModelNode model, FarmDefinition definition)
        {
            return AddHostFarm(model, definition, null);
        }

        public static FarmModelNode AddHostFarm(this FarmModelNode model, FarmDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
