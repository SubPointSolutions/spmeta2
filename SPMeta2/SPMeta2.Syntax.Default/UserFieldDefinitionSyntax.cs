using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class UserFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddUserField(this ModelNode model, UserFieldDefinition definition)
        {
            return AddUserField(model, definition, null);
        }

        public static ModelNode AddUserField(this ModelNode model, UserFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
