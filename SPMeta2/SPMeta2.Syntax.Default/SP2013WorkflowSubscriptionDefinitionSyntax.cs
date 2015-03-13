using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SP2013WorkflowSubscriptionDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSP2013WorkflowSubscription(this ModelNode model, SP2013WorkflowSubscriptionDefinition definition)
        {
            return AddSP2013WorkflowSubscription(model, definition, null);
        }

        public static ModelNode AddSP2013WorkflowSubscription(this ModelNode model, SP2013WorkflowSubscriptionDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddSP2013WorkflowSubscriptions(this ModelNode model, IEnumerable<SP2013WorkflowSubscriptionDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
