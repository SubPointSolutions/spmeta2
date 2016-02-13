using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class UserCustomActionModelNode : TypedModelNode
    {

    }

    public static class UserCustomActionSyntax
    {
        #region methods

        public static TModelNode AddUserCustomAction<TModelNode>(this TModelNode model, UserCustomActionDefinition definition)
            where TModelNode : ModelNode, IUserCustomActionHostModelNode, new()
        {
            return AddUserCustomAction(model, definition, null);
        }

        public static TModelNode AddUserCustomAction<TModelNode>(this TModelNode model, UserCustomActionDefinition definition,
            Action<UserCustomActionModelNode> action)
            where TModelNode : ModelNode, IUserCustomActionHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddUserCustomActions<TModelNode>(this TModelNode model, IEnumerable<UserCustomActionDefinition> definitions)
           where TModelNode : ModelNode, IUserCustomActionHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
