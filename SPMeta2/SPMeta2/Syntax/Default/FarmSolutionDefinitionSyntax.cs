using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class FarmSolutionModelNode : ListItemModelNode
    {

    }

    public static class FarmSolutionDefinitionSyntax
    {
        #region methods

        public static FarmModelNode AddFarmSolution(this FarmModelNode model, FarmSolutionDefinition definition)
        {
            return AddFarmSolution(model, definition, null);
        }

        public static FarmModelNode AddFarmSolution(this FarmModelNode model, FarmSolutionDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region methods

        //public static ModelNode AddHostFarmSolution(this ModelNode model, FarmSolutionDefinition definition)
        //{
        //    return AddHostFarmSolution(model, definition, null);
        //}

        //public static ModelNode AddHostFarmSolution(this ModelNode model, FarmSolutionDefinition definition, Action<ModelNode> action)
        //{
        //    return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        //}

        #endregion
    }
}
