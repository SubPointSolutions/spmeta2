using SPMeta2.Models;
using SPMeta2.Nintex.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Nintex.Syntax
{
    public static class ModelWithNamespaces
    {
        #region methods

        public static ModelNode WithNintexWorkflows(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode AddNintexWorkflow(this ModelNode model, NintexWorkflowDefinition definition)
        {
            return AddNintexWorkflow(model, definition, null);
        }

        public static ModelNode AddNintexWorkflow(this ModelNode model, NintexWorkflowDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
