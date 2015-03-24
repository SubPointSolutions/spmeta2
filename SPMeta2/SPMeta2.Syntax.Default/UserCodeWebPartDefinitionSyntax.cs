using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class UserCodeWebPartDefinitionSyntax
    {
        #region methods

        public static ModelNode AddUserCodeWebPart(this ModelNode model, UserCodeWebPartDefinition definition)
        {
            return AddUserCodeWebPart(model, definition, null);
        }

        public static ModelNode AddUserCodeWebPart(this ModelNode model, UserCodeWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddUserCodeWebParts(this ModelNode model, IEnumerable<UserCodeWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
