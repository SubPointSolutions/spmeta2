using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class ListDefinitionSyntax
    {
        #region methods

        #region add list

        public static ModelNode AddList(this ModelNode model, ListDefinition listDefinition)
        {
            return AddList(model, listDefinition, null);
        }

        public static ModelNode AddList(this ModelNode model, DefinitionBase listModel, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = listModel };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        #endregion

        #region model

        //public static IEnumerable<ListDefinition> GetLists(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<ListDefinition>();
        //}

        #endregion

        #endregion
    }
}
