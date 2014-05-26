using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class ListViewDefinitionSyntax
    {
        #region methods

        public static ModelNode AddView(this ModelNode model, ListViewDefinition listViewDefinition)
        {
            return AddView(model, listViewDefinition, null);
        }

        public static ModelNode AddView(this ModelNode model, DefinitionBase listViewDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = listViewDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        #endregion

        #region model

        //public static IEnumerable<ListViewDefinition> GetListViews(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<ListViewDefinition>();
        //}

        #endregion
    }
}
