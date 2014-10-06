using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class FarmSolutionDefinitionSyntax
    {
        #region methods

        public static ModelNode AddFarmSolution(this ModelNode model, FarmSolutionDefinition definition)
        {
            return AddFarmSolution(model, definition, null);
        }

        public static ModelNode AddFarmSolution(this ModelNode model, FarmSolutionDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
