using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class ContentTypeDefinitionSyntax
    {
        #region consts

        private const string ContentTypeGuidFormatString = "N";

        #endregion

        #region methods

        public static ModelNode AddContentType(this ModelNode siteModel, DefinitionBase contentTypeDefinition)
        {
            return AddContentType(siteModel, contentTypeDefinition, null);
        }

        public static ModelNode AddContentType(this ModelNode siteModel, DefinitionBase contentTypeDefinition, Action<ModelNode> contentTypeModelNodeAction)
        {
            var contentTypeModelNode = new ModelNode { Value = contentTypeDefinition };

            siteModel.ChildModels.Add(contentTypeModelNode);

            if (contentTypeModelNodeAction != null)
                contentTypeModelNodeAction(contentTypeModelNode);

            return siteModel;
        }

        public static ModelNode AddContentTypes(this ModelNode siteModel, params DefinitionBase[] contentTypeDefinitions)
        {
            return AddContentTypes(siteModel, (IEnumerable<DefinitionBase>)contentTypeDefinitions);
        }

        public static ModelNode AddContentTypes(this ModelNode siteModel, IEnumerable<DefinitionBase> contentTypeDefinitions)
        {
            foreach (var contentTypeDefinition in contentTypeDefinitions)
                AddContentType(siteModel, contentTypeDefinition);

            return siteModel;
        }

        public static string GetContentTypeId(this ContentTypeDefinition contentType)
        {
            if (contentType.Id != default(Guid))
            {
                // for content types like
                // 0x010100339210063E00144CBB5EFF79F55FE57400D05BFCF30398485FBBD6D5BB034AFF74
                // 0x010100339210063E00144CBB5EFF79F55FE57400D05BFCF30398485FBBD6D5BB034AFF74008FA22F0260524AF78AF78C349553F22E

                return contentType.ParentContentTypeId + "00" + contentType.Id.ToString(ContentTypeGuidFormatString);
            }

            if (!string.IsNullOrEmpty(contentType.IdNumberValue))
            {
                // for content types like
                // 0x010100339210063E00144CBB5EFF79F55FE574 
                // 0x010100339210063E00144CBB5EFF79F55FE57401 
                // 0x010100339210063E00144CBB5EFF79F55FE5740101

                return contentType.ParentContentTypeId + contentType.IdNumberValue;
            }

            // TODO
            // however, validation system for model definition are going to be implemented
            throw new ArgumentException("Either Id or IdNumberValue have to be specified for content type model");
        }

        #endregion
    }
}
