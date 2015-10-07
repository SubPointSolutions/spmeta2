using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class AlternateUrlModelNode : TypedModelNode
    {

    }

    public static class AlternateUrlDefinitionSyntax
    {
        #region methods

        public static TModelNode AddAlternateUrl<TModelNode>(this TModelNode model, AlternateUrlDefinition definition)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return AddAlternateUrl(model, definition, null);
        }

        public static TModelNode AddAlternateUrl<TModelNode>(this TModelNode model, AlternateUrlDefinition definition,
            Action<AlternateUrlModelNode> action)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddAlternateUrls<TModelNode>(this TModelNode model, IEnumerable<AlternateUrlDefinition> definitions)
           where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
