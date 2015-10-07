using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class FieldLinkDefinitionSyntax
    {
        #region methods

        public static TModelNode AddContentTypeFieldLinks<TModelNode>(this TModelNode model, IEnumerable<Guid> fieldIds)
            where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            foreach (var fieldId in fieldIds)
                AddContentTypeFieldLink(model, fieldId);

            return model;
        }

        public static TModelNode AddContentTypeFieldLink<TModelNode>(this TModelNode model, Guid fieldId)
             where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            return AddContentTypeFieldLink(model, fieldId, null);
        }

        public static TModelNode AddContentTypeFieldLink<TModelNode>(this TModelNode model, Guid fieldId,
            Action<ContentTypeFieldLinkModelNode> action)
            where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            return model.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
            {
                FieldId = fieldId
            }, action);
        }

        public static TModelNode AddContentTypeFieldLinks<TModelNode>(this TModelNode model, IEnumerable<FieldDefinition> definitions)
            where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            foreach (var definition in definitions)
                AddContentTypeFieldLink(model, definition);

            return model;
        }

        public static TModelNode AddContentTypeFieldLink<TModelNode>(this TModelNode model, FieldDefinition definition)
            where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            return AddContentTypeFieldLink(model, definition, null);
        }

        public static TModelNode AddContentTypeFieldLink<TModelNode>(this TModelNode model, FieldDefinition definition,
            Action<ContentTypeFieldLinkModelNode> action)
            where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            if (definition.Id != default(Guid))
            {
                return model.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                {
                    FieldId = definition.Id
                }, action);
            }

            return model.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
            {
                FieldInternalName = definition.InternalName
            }, action);
        }

        //public static ModelNode AddContentTypeFieldLink(this ModelNode model, ContentTypeFieldLinkDefinition definition)
        //{
        //    return model.AddDefinitionNode(definition, null);
        //}

        //public static ModelNode AddContentTypeFieldLink(this ModelNode model, ContentTypeFieldLinkDefinition definition, Action<ModelNode> action)
        //{
        //    return model.AddDefinitionNode(definition, action);
        //}

        #endregion

        #region array overload

        public static TModelNode AddContentTypeFieldLink<TModelNode>(this TModelNode model, IEnumerable<ContentTypeFieldLinkDefinition> definitions)
            where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

    }
}
