using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class ReusableHTMLItemModelNode : ListItemModelNode
    {

    }
    public static class ReusableHTMLItemDefinitionSyntax
    {
        #region methods

        public static TModelNode AddReusableHTMLItem<TModelNode>(this TModelNode model, ReusableHTMLItemDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddReusableHTMLItem(model, definition, null);
        }

        public static TModelNode AddReusableHTMLItem<TModelNode>(this TModelNode model, ReusableHTMLItemDefinition definition,
            Action<ReusableHTMLItemModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddReusableHTMLItems<TModelNode>(this TModelNode model, IEnumerable<ReusableHTMLItemDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
