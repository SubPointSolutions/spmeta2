using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class JobDefinitionSyntax
    {
        #region methods

        public static ModelNode AddJob(this ModelNode model, JobDefinition definition)
        {
            return AddJob(model, definition, null);
        }

        public static ModelNode AddJob(this ModelNode model, JobDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

       
    }
}
