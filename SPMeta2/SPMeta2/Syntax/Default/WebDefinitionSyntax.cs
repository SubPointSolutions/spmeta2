using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class WebModelNode : TypedModelNode,
        // should always be same as RootWebModelNode
        // Missing TopNavigationNodeDefinition Syntax for RootWebModelNode #953
        // https://github.com/SubPointSolutions/spmeta2/issues/953
        
        IWebModelNode,
        IFieldHostModelNode,
        IContentTypeHostModelNode,
        ISecurableObjectHostModelNode,
        IPropertyHostModelNode,
        IEventReceiverHostModelNode,
        IWebHostModelNode,
        IWelcomePageHostModelNode,
        IModuleFileHostModelNode,
        IAuditSettingsHostModelNode,
        IFeatureHostModelNode,
        IUserCustomActionHostModelNode,
        ITopNavigationNodeHostModelNode,
        IQuickLaunchNavigationNodeHostModelNode,
        ISP2013WorkflowSubscriptionHostModelNode,
        IWorkflowAssociationHostModelNode,
        ISearchSettingsHostModelNode
    {

    }

    public static class WebDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWeb<TModelNode>(this TModelNode model, WebDefinition definition)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddWeb(model, definition, null);
        }

        public static TModelNode AddWeb<TModelNode>(this TModelNode model, WebDefinition definition,
            Action<WebModelNode> action)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddWebs<TModelNode>(this TModelNode model, IEnumerable<WebDefinition> definitions)
           where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override

        public static TModelNode AddHostWeb<TModelNode>(this TModelNode model, WebDefinition definition)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddHostWeb(model, definition, null);
        }
        public static TModelNode AddHostWeb<TModelNode>(this TModelNode model, WebDefinition definition,
            Action<WebModelNode> action)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
