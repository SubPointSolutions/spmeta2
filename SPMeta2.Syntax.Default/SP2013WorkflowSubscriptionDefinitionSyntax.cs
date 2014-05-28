using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class SP2013WorkflowSubscriptionDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSP2013WorkflowSubscription(this ModelNode model, DefinitionBase workflowDefinitionModel)
        {
            return AddSP2013WorkflowSubscription(model, workflowDefinitionModel, null);
        }

        public static ModelNode AddSP2013WorkflowSubscription(this ModelNode model, DefinitionBase workflowDefinitionModel, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = workflowDefinitionModel };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        #endregion
    }
}
