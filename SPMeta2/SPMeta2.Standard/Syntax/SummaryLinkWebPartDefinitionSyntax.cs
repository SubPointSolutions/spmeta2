using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class SummaryLinkWebPartDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddSummaryLinkWebPart(this ModelNode model, SummaryLinkWebPartDefinition definition)
        {
            return AddSummaryLinkWebPart(model, definition, null);
        }

        public static ModelNode AddSummaryLinkWebPart(this ModelNode model, SummaryLinkWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddSummaryLinkWebParts(this ModelNode model, IEnumerable<SummaryLinkWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
