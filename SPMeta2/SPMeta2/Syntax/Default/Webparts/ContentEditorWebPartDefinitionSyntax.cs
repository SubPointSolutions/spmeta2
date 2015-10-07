using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class ContentEditorWebPartModelNode : WebPartModelNode
    {

    }

    public static class ContentEditorWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddContentEditorWebPart<TModelNode>(this TModelNode model, ContentEditorWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddContentEditorWebPart(model, definition, null);
        }

        public static TModelNode AddContentEditorWebPart<TModelNode>(this TModelNode model, ContentEditorWebPartDefinition definition,
            Action<ContentEditorWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddContentEditorWebParts<TModelNode>(this TModelNode model, IEnumerable<ContentEditorWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
