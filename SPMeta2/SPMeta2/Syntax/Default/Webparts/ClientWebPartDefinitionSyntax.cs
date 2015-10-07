using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class ClientWebPartModelNode : WebPartModelNode
    {

    }

    public static class ClientWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddClientWebPart<TModelNode>(this TModelNode model, ClientWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddClientWebPart(model, definition, null);
        }

        public static TModelNode AddClientWebPart<TModelNode>(this TModelNode model, ClientWebPartDefinition definition,
            Action<ClientWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddClientWebParts<TModelNode>(this TModelNode model, IEnumerable<ClientWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
