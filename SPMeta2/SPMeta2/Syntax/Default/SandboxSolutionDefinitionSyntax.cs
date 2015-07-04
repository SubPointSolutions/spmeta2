using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class SandboxSolutionModelNode : TypedModelNode
    {

    }

    public static class SandboxSolutionDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddSandboxSolution(this SiteModelNode model, SandboxSolutionDefinition definition)
        {
            return AddSandboxSolution(model, definition, null);
        }

        public static SiteModelNode AddSandboxSolution(this SiteModelNode model, SandboxSolutionDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddSandboxSolutions(this ModelNode model, IEnumerable<SandboxSolutionDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
