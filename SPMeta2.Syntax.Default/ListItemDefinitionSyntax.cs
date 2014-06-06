using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Syntax.Default
{
    public static class ListItemDefinitionSyntax
    {
        #region methods

        public static ModelNode AddListItem(this ModelNode model, ListItemDefinition listItemDefinition)
        {
            return AddListItem(model, listItemDefinition, null);
        }

        public static ModelNode AddListItem(this ModelNode model, ListItemDefinition fieldValueDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = fieldValueDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }


        #endregion
    }
}
