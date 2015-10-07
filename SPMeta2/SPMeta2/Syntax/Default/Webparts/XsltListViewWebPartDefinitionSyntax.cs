using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class XsltListViewWebPartModelNode : WebPartModelNode
    {

    }

    public static class XsltListViewWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddXsltListViewWebPart<TModelNode>(this TModelNode model, XsltListViewWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddXsltListViewWebPart(model, definition, null);
        }

        public static TModelNode AddXsltListViewWebPart<TModelNode>(this TModelNode model, XsltListViewWebPartDefinition definition,
            Action<XsltListViewWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddXsltListViewWebParts<TModelNode>(this TModelNode model, IEnumerable<XsltListViewWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
