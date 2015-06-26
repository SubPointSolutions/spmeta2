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

        public static WebPartPageModelNode AddDeleteWebParts(this WebPartPageModelNode model, DeleteWebPartsDefinition definition)
        {
            return AddDeleteWebParts(model, definition, null);
        }

        public static WebPartPageModelNode AddDeleteWebParts(this WebPartPageModelNode model, DeleteWebPartsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WikiPageModelNode AddDeleteWebParts(this WikiPageModelNode model, DeleteWebPartsDefinition definition)
        {
            return AddDeleteWebParts(model, definition, null);
        }

        public static WikiPageModelNode AddDeleteWebParts(this WikiPageModelNode model, DeleteWebPartsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload


        #endregion
    }
}
