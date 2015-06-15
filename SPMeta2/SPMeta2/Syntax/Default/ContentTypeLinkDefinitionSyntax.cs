using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;

namespace SPMeta2.Syntax.Default
{
    public static class ContentTypeLinkDefinitionSyntax
    {
        #region methods

        public static ModelNode AddContentTypeLink(this ModelNode model, ContentTypeLinkDefinition definition)
        {
            return AddContentTypeLink(model, definition, null);
        }

        public static ModelNode AddContentTypeLink(this ModelNode model, ContentTypeLinkDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #region array overload

        public static ModelNode AddContentTypeLinks(this ModelNode model, IEnumerable<ContentTypeLinkDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static ModelNode AddContentTypeLink(this ModelNode model, string contentTypdId)
        {
            return AddContentTypeLink(model, new ContentTypeLinkDefinition
            {
                ContentTypeId = contentTypdId,
                ContentTypeName = string.Empty
            });
        }

        public static ModelNode AddContentTypeLink(this ModelNode model, ContentTypeDefinition definition)
        {
            return AddContentTypeLink(model, definition, null);
        }

        public static ModelNode AddContentTypeLink(this ModelNode model, ContentTypeDefinition definition, Action<ModelNode> action)
        {
            return AddContentTypeLink(model, new ContentTypeLinkDefinition
            {
                ContentTypeId = definition.GetContentTypeId(),
                ContentTypeName = definition.Name
            }, action);
        }

        #endregion

       
    }
}
