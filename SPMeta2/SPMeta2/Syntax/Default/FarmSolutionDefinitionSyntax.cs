using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class FarmSolutionModelNode : ListItemModelNode
    {

    }

    public static class FarmSolutionDefinitionSyntax
    {
        #region methods

        public static TModelNode AddFarmSolution<TModelNode>(this TModelNode model, FarmSolutionDefinition definition)
            where TModelNode : ModelNode, IFarmSolutionModelHostNode, new()
        {
            return AddFarmSolution(model, definition, null);
        }

        public static TModelNode AddFarmSolution<TModelNode>(this TModelNode model, FarmSolutionDefinition definition,
            Action<FarmSolutionModelNode> action)
            where TModelNode : ModelNode, IFarmSolutionModelHostNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddFarmSolutions<TModelNode>(this TModelNode model, IEnumerable<FarmSolutionDefinition> definitions)
           where TModelNode : ModelNode, IFarmSolutionModelHostNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
