using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ContentTypeModelNode : TypedModelNode, IContentTypeModelNode,
        IEventReceiverHostModelNode,
        IModuleFileHostModelNode,
        IContentTypeFieldLinkHostModelNode
    {

    }

    public static class ContentTypeDefinitionSyntax
    {
        #region consts

        private const string ContentTypeGuidFormatString = "N";

        #endregion

        #region methods

        public static TModelNode AddContentType<TModelNode>(this TModelNode model, ContentTypeDefinition definition)
            where TModelNode : ModelNode, IContentTypeHostModelNode, new()
        {
            return AddContentType(model, definition, null);
        }

        public static TModelNode AddContentType<TModelNode>(this TModelNode model, ContentTypeDefinition definition,
            Action<ContentTypeModelNode> action)
            where TModelNode : ModelNode, IContentTypeHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddContentTypes<TModelNode>(this TModelNode model, IEnumerable<ContentTypeDefinition> definitions)
           where TModelNode : ModelNode, IContentTypeHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region utils

        public static bool IsChildOf(this ContentTypeDefinition childContentTypeDefinition,
            ContentTypeDefinition parentContentTypeDefinition)
        {
            return IsChildOf(childContentTypeDefinition.GetContentTypeId(), parentContentTypeDefinition.GetContentTypeId());
        }

        public static bool IsChildOf(this ContentTypeDefinition childContentTypeDefinition,
            string parentContentTypId)
        {
            return IsChildOf(childContentTypeDefinition.GetContentTypeId(), parentContentTypId);
        }

        public static bool IsChildOf(string childId, string parentId)
        {
            if (parentId.Length < childId.Length)
                return false;

            for (int i = 0; i < childId.Length; i++)
                if (childId[i] != parentId[i])
                    return false;

            return true;
        }

        public static string GetContentTypeId(this ContentTypeDefinition contentType)
        {
            if (contentType.Id != default(Guid))
            {
                // for content types like
                // 0x010100339210063E00144CBB5EFF79F55FE57400D05BFCF30398485FBBD6D5BB034AFF74
                // 0x010100339210063E00144CBB5EFF79F55FE57400D05BFCF30398485FBBD6D5BB034AFF74008FA22F0260524AF78AF78C349553F22E

                return contentType.ParentContentTypeId + "00" + contentType.Id.ToString(ContentTypeGuidFormatString).ToUpper();
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

        #region add host

        public static TModelNode AddHostContentType<TModelNode>(this TModelNode model, ContentTypeDefinition definition)
             where TModelNode : ModelNode, IContentTypeHostModelNode, new()
        {
            return AddHostContentType(model, definition, null);
        }
        public static TModelNode AddHostContentType<TModelNode>(this TModelNode model, ContentTypeDefinition definition,
            Action<ContentTypeModelNode> action)
            where TModelNode : ModelNode, IContentTypeHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
