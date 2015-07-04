using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class JobModelNode : TypedModelNode
    {

    }

    public static class JobDefinitionSyntax
    {
        #region methods

        public static WebApplicationModelNode AddJob(this WebApplicationModelNode model, JobDefinition definition)
        {
            return AddJob(model, definition, null);
        }

        public static WebApplicationModelNode AddJob(this WebApplicationModelNode model, JobDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

       
    }
}
