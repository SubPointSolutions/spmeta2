using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SandboxSolutionModelNode : TypedModelNode
    {

    }

    public static class SandboxSolutionDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSandboxSolution<TModelNode>(this TModelNode model, SandboxSolutionDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddSandboxSolution(model, definition, null);
        }

        public static TModelNode AddSandboxSolution<TModelNode>(this TModelNode model, SandboxSolutionDefinition definition,
            Action<SandboxSolutionModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSandboxSolutions<TModelNode>(this TModelNode model, IEnumerable<SandboxSolutionDefinition> definitions)
           where TModelNode : ModelNode, ISiteModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
