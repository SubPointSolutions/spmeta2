using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SP2013WorkflowDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSP2013Workflow(this ModelNode model, SP2013WorkflowDefinition definition)
        {
            return AddSP2013Workflow(model, definition, null);
        }

        public static ModelNode AddSP2013Workflow(this ModelNode model, SP2013WorkflowDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddSP2013Workflows(this ModelNode model, IEnumerable<SP2013WorkflowDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
