using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class WebPartModelNode : TypedModelNode
    {

    }

    public static class WebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWebPart<TModelNode>(this TModelNode model, WebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddWebPart(model, definition, null);
        }

        public static TModelNode AddWebPart<TModelNode>(this TModelNode model, WebPartDefinition definition,
            Action<WebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddWebParts<TModelNode>(this TModelNode model, IEnumerable<WebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
