using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class HtmlMasterPageModelNode : ListItemModelNode
    {

    }

    public static class HtmlMasterPageDefinitionSyntax
    {
        #region methods

        public static TModelNode AddHtmlMasterPage<TModelNode>(this TModelNode model, HtmlMasterPageDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddHtmlMasterPage(model, definition, null);
        }

        public static TModelNode AddHtmlMasterPage<TModelNode>(this TModelNode model, HtmlMasterPageDefinition definition,
            Action<HtmlMasterPageModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddHtmlMasterPages<TModelNode>(this TModelNode model, IEnumerable<HtmlMasterPageDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
