using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class FieldLinkDefinitionSyntax
    {
        #region methods

        public static ModelNode AddContentTypeFieldLinks(this ModelNode model, IEnumerable<Guid> fieldIds)
        {
            foreach (var fieldId in fieldIds)
                AddContentTypeFieldLink(model, fieldId);

            return model;
        }

        public static ModelNode AddContentTypeFieldLinks(this ModelNode model, params Guid[] fieldIds)
        {
            foreach (var fieldId in fieldIds)
                AddContentTypeFieldLink(model, fieldId);

            return model;
        }

        public static ModelNode AddContentTypeFieldLink(this ModelNode model, Guid fieldId)
        {
            return AddContentTypeFieldLink(model, fieldId, null);
        }

        public static ModelNode AddContentTypeFieldLink(this ModelNode model, Guid fieldId, Action<ModelNode> action)
        {
            return model.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
            {
                FieldId = fieldId
            }, action);
        }

        public static ModelNode AddContentTypeFieldLinks(this ModelNode model, IEnumerable<FieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                AddContentTypeFieldLink(model, definition);

            return model;
        }

        public static ModelNode AddContentTypeFieldLinks(this ModelNode model, params FieldDefinition[] definitions)
        {
            foreach (var definition in definitions)
                AddContentTypeFieldLink(model, definition);

            return model;
        }

        public static ModelNode AddContentTypeFieldLink(this ModelNode model, FieldDefinition definition)
        {
            return AddContentTypeFieldLink(model, definition, null);
        }

        public static ModelNode AddContentTypeFieldLink(this ModelNode model, FieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
            {
                FieldId = definition.Id
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


    }
}
