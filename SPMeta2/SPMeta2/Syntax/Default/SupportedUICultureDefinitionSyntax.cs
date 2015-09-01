using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SupportedUICultureModelNode : TypedModelNode
    {

    }

    public static class SupportedUICultureDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSupportedUICulture<TModelNode>(this TModelNode model, SupportedUICultureDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddSupportedUICulture(model, definition, null);
        }

        public static TModelNode AddSupportedUICulture<TModelNode>(this TModelNode model, SupportedUICultureDefinition definition,
            Action<SupportedUICultureModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSupportedUICultures<TModelNode>(this TModelNode model, IEnumerable<SupportedUICultureDefinition> definitions)
           where TModelNode : ModelNode, IWebModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
