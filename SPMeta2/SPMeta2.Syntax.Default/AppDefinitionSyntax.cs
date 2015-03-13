using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class AppDefinitionSyntax
    {
        #region methods

        public static ModelNode AddApp(this ModelNode model, AppDefinition definition)
        {
            return AddApp(model, definition, null);
        }

        public static ModelNode AddApp(this ModelNode model, AppDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddApps(this ModelNode model, IEnumerable<AppDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
