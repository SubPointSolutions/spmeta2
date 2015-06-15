using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class WebApplicationDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWebApplication(this ModelNode model, WebApplicationDefinition definition)
        {
            return AddWebApplication(model, definition, null);
        }

        public static ModelNode AddWebApplication(this ModelNode model, WebApplicationDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }
 
        #endregion

        #region add host

        public static ModelNode AddHostWebApplication(this ModelNode model, WebApplicationDefinition definition)
        {
            return AddHostWebApplication(model, definition, null);
        }

        public static ModelNode AddHostWebApplication(this ModelNode model, WebApplicationDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        } 

        #endregion
    }
}
