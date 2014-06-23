using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class WebPartDefinitionSyntax
    {
        #region methods


        public static ModelNode AddWebPart(this ModelNode model, WebPartDefinition webPartDefinition)
        {
            return AddWebPart(model, webPartDefinition, null);
        }

        public static ModelNode AddWebPart(this ModelNode model, WebPartDefinition webPartDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = webPartDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        #endregion

        #region model

        //public static IEnumerable<WebPartDefinition> GetWebParts(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<WebPartDefinition>();
        //}

        #endregion
    }
}
