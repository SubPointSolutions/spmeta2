using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class UserCodeWebPartModelNode : WebPartModelNode
    {

    }
    public static class UserCodeWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddUserCodeWebPart<TModelNode>(this TModelNode model, UserCodeWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddUserCodeWebPart(model, definition, null);
        }

        public static TModelNode AddUserCodeWebPart<TModelNode>(this TModelNode model, UserCodeWebPartDefinition definition,
            Action<UserCodeWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddUserCodeWebParts<TModelNode>(this TModelNode model, IEnumerable<UserCodeWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
