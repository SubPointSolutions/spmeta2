﻿using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class FarmModelNode : TypedModelNode, IFarmModelNode,
        IPropertyHostModelNode,
        IManagedPropertyHostModelNode,
        IFeatureHostModelNode,
        IWebApplicationHostModelNode,
        ISecureStoreApplicationHostModelNode
    {

    }

    //public static class FarmDefinitionSyntax
    //{
    //    #region methods

    //    public static FarmModelNode AddFarm(this FarmModelNode model, FarmDefinition definition)
    //    {
    //        return AddFarm(model, definition, null);
    //    }

    //    public static FarmModelNode AddFarm(this FarmModelNode model, FarmDefinition definition, Action<ModelNode> action)
    //    {
    //        return model.AddTypedDefinitionNode(definition, action);
    //    }

    //    #endregion

    //    #region methods

    //    public static FarmModelNode AddHostFarm(this FarmModelNode model, FarmDefinition definition)
    //    {
    //        return AddHostFarm(model, definition, null);
    //    }

    //    public static FarmModelNode AddHostFarm(this FarmModelNode model, FarmDefinition definition, Action<ModelNode> action)
    //    {
    //        return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
    //    }

    //    #endregion
    //}
}
