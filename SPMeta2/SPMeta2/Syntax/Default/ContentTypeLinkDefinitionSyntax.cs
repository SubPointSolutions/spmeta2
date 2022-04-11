using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ContentTypeLinkModelNode : TypedModelNode
        , IContentTypeModelNode
        , IWorkflowAssociationHostModelNode
    {

    }

    public static class ContentTypeLinkDefinitionSyntax
    {
        #region methods

        public static TModelNode AddContentTypeLink<TModelNode>(this TModelNode model, ContentTypeLinkDefinition definition)
            where TModelNode : ModelNode, IContentTypeLinkHostModelNode, new()
        {
            return AddContentTypeLink(model, definition, null);
        }

        public static TModelNode AddContentTypeLink<TModelNode>(this TModelNode model, ContentTypeLinkDefinition definition,
            Action<ContentTypeLinkModelNode> action)
            where TModelNode : ModelNode, IContentTypeLinkHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddContentTypeLinks<TModelNode>(this TModelNode model, IEnumerable<ContentTypeLinkDefinition> definitions)
           where TModelNode : ModelNode, IContentTypeLinkHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region additions

        public static TModelNode AddContentTypeLink<TModelNode>(this TModelNode model, string contentTypdId)
           where TModelNode : ModelNode, IContentTypeLinkHostModelNode, new()
        {
            return AddContentTypeLink(model, new ContentTypeLinkDefinition
            {
                ContentTypeId = contentTypdId,
                ContentTypeName = string.Empty
            });
        }

        public static TModelNode AddContentTypeLink<TModelNode>(this TModelNode model, ContentTypeDefinition definition)
           where TModelNode : ModelNode, IContentTypeLinkHostModelNode, new()
        {
            return AddContentTypeLink(model, definition, null);
        }

        public static TModelNode AddContentTypeLink<TModelNode>(this TModelNode model, ContentTypeDefinition definition,
          Action<ContentTypeLinkModelNode> action)
          where TModelNode : ModelNode, IContentTypeLinkHostModelNode, new()
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
