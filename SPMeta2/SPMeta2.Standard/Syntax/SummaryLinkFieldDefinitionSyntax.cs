using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class SummaryLinkFieldDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddSummaryLinkField(this ModelNode model, SummaryLinkFieldDefinition definition)
        {
            return AddSummaryLinkField(model, definition, null);
        }

        public static ModelNode AddSummaryLinkField(this ModelNode model, SummaryLinkFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddSummaryLinkFields(this ModelNode model, IEnumerable<SummaryLinkFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
