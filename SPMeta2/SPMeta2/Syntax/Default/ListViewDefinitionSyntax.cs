using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class ListViewModelNode : TypedModelNode
    {

    }

    public static class ListViewDefinitionSyntax
    {
        #region methods

        public static TModelNode AddListView<TModelNode>(this TModelNode model, ListViewDefinition definition)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return AddListView(model, definition, null);
        }

        public static TModelNode AddListView<TModelNode>(this TModelNode model, ListViewDefinition definition,
            Action<ListViewModelNode> action)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddListViews<TModelNode>(this TModelNode model, IEnumerable<ListViewDefinition> definitions)
           where TModelNode : ModelNode, IListModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override



        #endregion
    }
}
