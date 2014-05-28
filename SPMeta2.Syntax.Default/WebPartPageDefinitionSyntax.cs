using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class WebPartPageDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWebPartPage(this ModelNode model, WebPartPageDefinition webpartPageDefinition)
        {
            return AddWebPartPage(model, webpartPageDefinition, null);
        }

        public static ModelNode AddWebPartPage(this ModelNode model, DefinitionBase webpartPageDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = webpartPageDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        #endregion

        #region model

        //public static IEnumerable<WebPartPageDefinition> GetWebPartPages(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<WebPartPageDefinition>();
        //}

        #endregion
    }
}
