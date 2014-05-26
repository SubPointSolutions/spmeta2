using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class WikiPageDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWikiPage(this ModelNode model, WikiPageDefinition wikiPageDefinition)
        {
            return AddWikiPage(model, wikiPageDefinition, null);
        }

        public static ModelNode AddWikiPage(this ModelNode model, WikiPageDefinition webpartPageDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = webpartPageDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        #endregion

        #region model

        //public static IEnumerable<WikiPageDefinition> GetWikiPages(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<WikiPageDefinition>();
        //}

        #endregion
    }
}
