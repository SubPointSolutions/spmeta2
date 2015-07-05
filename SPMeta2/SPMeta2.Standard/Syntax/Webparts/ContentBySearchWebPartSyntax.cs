using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class ContentBySearchWebPartModelNode : WebPartModelNode
    {

    }
    public static class ContentBySearchWebPartSyntax
    {
        #region methods

        public static TModelNode AddContentBySearchWebPart<TModelNode>(this TModelNode model, ContentBySearchWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddContentBySearchWebPart(model, definition, null);
        }

        public static TModelNode AddContentBySearchWebPart<TModelNode>(this TModelNode model, ContentBySearchWebPartDefinition definition,
            Action<ContentBySearchWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddContentBySearchWebParts<TModelNode>(this TModelNode model, IEnumerable<ContentBySearchWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
