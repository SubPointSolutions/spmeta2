using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class ListItemFieldValuesModelNode : TypedModelNode
    {

    }

    public static class ListItemFieldValuesDefinitionSyntax
    {
        #region methods

        public static ListItemModelNode AddListItemFieldValues(this ListItemModelNode model, ListItemFieldValuesDefinition definition)
        {
            return AddListItemFieldValues(model, definition, null);
        }

        public static ListItemModelNode AddListItemFieldValues(this ListItemModelNode model, ListItemFieldValuesDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ListItemModelNode AddListItemFieldValues(this ListItemModelNode model, IEnumerable<ListItemFieldValuesDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
      
    }
}
