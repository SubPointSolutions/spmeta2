using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class WebApplicationModelNode : TypedModelNode,
        IPropertyBagHostModelNode,
        IFeatureHostModelNode
    {



    }

    public static class WebApplicationDefinitionSyntax
    {
        #region methods

        public static FarmModelNode AddWebApplication(this FarmModelNode model, WebApplicationDefinition definition)
        {
            return AddWebApplication(model, definition, null);
        }

        public static FarmModelNode AddWebApplication(this FarmModelNode model, WebApplicationDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region add host

        public static FarmModelNode AddHostWebApplication(this FarmModelNode model, WebApplicationDefinition definition)
        {
            return AddHostWebApplication(model, definition, null);
        }

        public static FarmModelNode AddHostWebApplication(this FarmModelNode model, WebApplicationDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
