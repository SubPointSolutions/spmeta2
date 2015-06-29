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

        public static WebModelNode AddApp(this WebModelNode model, AppDefinition definition)
        {
            return AddApp(model, definition, null);
        }

        public static WebModelNode AddApp(this WebModelNode model, AppDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static WebModelNode AddApps(this WebModelNode model, IEnumerable<AppDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
