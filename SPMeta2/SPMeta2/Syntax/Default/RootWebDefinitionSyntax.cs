using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class RootWebModelNode : TypedModelNode,
        // should always replicate WebModelNode syntax
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

    public static class RootWebDefinitionSyntax
    {
        #region methods

        public static TModelNode AddRootWeb<TModelNode>(this TModelNode model, RootWebDefinition definition)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddRootWeb(model, definition, null);
        }

        public static TModelNode AddRootWeb<TModelNode>(this TModelNode model, RootWebDefinition definition,
            Action<RootWebModelNode> action)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload


        #endregion

        #region host override

        public static TModelNode AddHostRootWeb<TModelNode>(this TModelNode model, RootWebDefinition definition)
             where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddHostRootWeb(model, definition, null);
        }
        public static TModelNode AddHostRootWeb<TModelNode>(this TModelNode model, RootWebDefinition definition,
            Action<RootWebModelNode> action)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
