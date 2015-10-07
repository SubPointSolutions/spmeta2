using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class AppPrincipalModelNode : TypedModelNode
    {

    }

    public static class AppPrincipalDefinitionSyntax
    {
        #region methods

        public static TModelNode AddAppPrincipal<TModelNode>(this TModelNode model, AppPrincipalDefinition definition)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddAppPrincipal(model, definition, null);
        }

        public static TModelNode AddAppPrincipal<TModelNode>(this TModelNode model, AppPrincipalDefinition definition,
            Action<AppPrincipalModelNode> action)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddAppPrincipals<TModelNode>(this TModelNode model, IEnumerable<AppPrincipalDefinition> definitions)
           where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
