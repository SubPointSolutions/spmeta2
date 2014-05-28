using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class WebDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWeb(this ModelNode model, WebDefinition webDefinition)
        {
            return AddWeb(model, webDefinition, null);
        }

        public static ModelNode AddWeb(this ModelNode model, WebDefinition webDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = webDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        #endregion
    }
}
