using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class UserCustomActionSyntax
    {
        #region methods

        public static ModelNode AddUserCustomAction(this ModelNode model, UserCustomActionDefinition definition)
        {
            return AddUserCustomAction(model, definition, null);
        }

        public static ModelNode AddUserCustomAction(this ModelNode model, UserCustomActionDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddUserCustomActions(this ModelNode model, IEnumerable<UserCustomActionDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
