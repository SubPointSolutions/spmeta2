using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class ImageWebPartModelNode : WebPartModelNode
    {

    }

    public static class ImageWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddImageWebPart<TModelNode>(this TModelNode model, ImageWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddImageWebPart(model, definition, null);
        }

        public static TModelNode AddImageWebPart<TModelNode>(this TModelNode model, ImageWebPartDefinition definition,
            Action<ImageWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddImageWebParts<TModelNode>(this TModelNode model, IEnumerable<ImageWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
