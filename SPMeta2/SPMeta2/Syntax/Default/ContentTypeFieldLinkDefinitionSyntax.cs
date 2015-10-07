using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ContentTypeFieldLinkModelNode : TypedModelNode
    {

    }

    public static class ContentTypeFieldLinkDefinitionSyntax
    {
        #region methods

        public static TModelNode AddContentTypeFieldLink<TModelNode>(this TModelNode model, ContentTypeFieldLinkDefinition definition)
            where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            return AddContentTypeFieldLink(model, definition, null);
        }

        public static TModelNode AddContentTypeFieldLink<TModelNode>(this TModelNode model, ContentTypeFieldLinkDefinition definition,
            Action<ContentTypeFieldLinkModelNode> action)
            where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddContentTypeFieldLinks<TModelNode>(this TModelNode model, IEnumerable<ContentTypeFieldLinkDefinition> definitions)
           where TModelNode : ModelNode, IContentTypeFieldLinkHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }

}
