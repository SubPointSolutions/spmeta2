using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class ContentTypeLinkDefinitionSyntax
    {
        #region methods

        public static ModelNode AddContentTypeLink(this ModelNode model, ContentTypeLinkDefinition contentTypeLinkDefinition)
        {
            return AddContentTypeLink(model, contentTypeLinkDefinition, null);
        }

        public static ModelNode AddContentTypeLink(this ModelNode model, ContentTypeLinkDefinition contentTypeLinkModel, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = contentTypeLinkModel };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        public static ModelNode AddContentTypeLink(this ModelNode model, ContentTypeDefinition contentTypeDefinition)
        {
            return AddContentTypeLink(model, new ContentTypeLinkDefinition
            {
                ContentTypeId = contentTypeDefinition.GetContentTypeId(),
                ContentTypeName = contentTypeDefinition.Name
            });
        }

        //public static IEnumerable<ContentTypeLinkDefinition> GetContentTypeLinks(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<ContentTypeLinkDefinition>();
        //}

        #endregion
    }
}
