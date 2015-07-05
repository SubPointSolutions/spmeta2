using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class SummaryLinkWebPartModelNode : WebPartModelNode
    {

    }

    public static class SummaryLinkWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSummaryLinkWebPart<TModelNode>(this TModelNode model, SummaryLinkWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddSummaryLinkWebPart(model, definition, null);
        }

        public static TModelNode AddSummaryLinkWebPart<TModelNode>(this TModelNode model, SummaryLinkWebPartDefinition definition,
            Action<SummaryLinkWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSummaryLinkWebParts<TModelNode>(this TModelNode model, IEnumerable<SummaryLinkWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
