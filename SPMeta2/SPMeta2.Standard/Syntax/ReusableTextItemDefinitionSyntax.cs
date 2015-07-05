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
    public class ReusableTextItemModelNode : ListItemModelNode
    {

    }

    public static class ReusableTextItemDefinitionSyntax
    {
        #region methods

        public static TModelNode AddReusableTextItem<TModelNode>(this TModelNode model, ReusableTextItemDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddReusableTextItem(model, definition, null);
        }

        public static TModelNode AddReusableTextItem<TModelNode>(this TModelNode model, ReusableTextItemDefinition definition,
            Action<ReusableTextItemModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddReusableTextItems<TModelNode>(this TModelNode model, IEnumerable<ReusableTextItemDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
