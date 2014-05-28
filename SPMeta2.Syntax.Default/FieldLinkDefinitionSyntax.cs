using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;

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
            return AddContentTypeFieldLink(model, new ContentTypeFieldLinkDefinition
            {
                FieldId = fieldId
            });
        }

        public static ModelNode AddContentTypeFieldLinks(this ModelNode model, IEnumerable<FieldDefinition> fieldDefinitions)
        {
            foreach (var fieldDefinition in fieldDefinitions)
                AddContentTypeFieldLink(model, fieldDefinition);

            return model;
        }

        public static ModelNode AddContentTypeFieldLinks(this ModelNode model, params FieldDefinition[] fieldDefinitions)
        {
            foreach (var fieldDefinition in fieldDefinitions)
                AddContentTypeFieldLink(model, fieldDefinition);

            return model;
        }

        public static ModelNode AddContentTypeFieldLink(this ModelNode model, FieldDefinition fieldDefinition)
        {
            return AddContentTypeFieldLink(model, new ContentTypeFieldLinkDefinition
            {
                FieldId = fieldDefinition.Id
            });
        }

        public static ModelNode AddContentTypeFieldLink(this ModelNode model, DefinitionBase contentTypeFieldLink)
        {
            model.ChildModels.Add(new ModelNode { Value = contentTypeFieldLink });

            return model;
        }




        #endregion

        #region model

        

        #endregion
    }
}
