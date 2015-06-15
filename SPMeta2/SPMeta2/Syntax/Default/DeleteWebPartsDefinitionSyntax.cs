using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class DeleteWebPartsDefinitionSyntax
    {
        #region methods

        public static ModelNode AddDeleteWebParts(this ModelNode model, DeleteWebPartsDefinition definition)
        {
            return AddDeleteWebParts(model, definition, null);
        }

        public static ModelNode AddDeleteWebParts(this ModelNode model, DeleteWebPartsDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload


        #endregion
    }
}
