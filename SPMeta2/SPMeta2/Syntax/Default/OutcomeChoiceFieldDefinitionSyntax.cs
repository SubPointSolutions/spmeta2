using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class OutcomeChoiceFieldDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddOutcomeChoiceField(this SiteModelNode model, OutcomeChoiceFieldDefinition definition)
        {
            return AddOutcomeChoiceField(model, definition, null);
        }

        public static SiteModelNode AddOutcomeChoiceField(this SiteModelNode model, OutcomeChoiceFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddOutcomeChoiceField(this WebModelNode model, OutcomeChoiceFieldDefinition definition)
        {
            return AddOutcomeChoiceField(model, definition, null);
        }

        public static WebModelNode AddOutcomeChoiceField(this WebModelNode model, OutcomeChoiceFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ListModelNode AddOutcomeChoiceField(this ListModelNode model, OutcomeChoiceFieldDefinition definition)
        {
            return AddOutcomeChoiceField(model, definition, null);
        }

        public static ListModelNode AddOutcomeChoiceField(this ListModelNode model, OutcomeChoiceFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddOutcomeChoiceFields(this ModelNode model, IEnumerable<OutcomeChoiceFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
