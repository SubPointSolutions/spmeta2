using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class WebModelNode : TypedModelNode, IWebModelNode,
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

        public static SiteModelNode AddHostWeb(this SiteModelNode model, WebDefinition definition)
        {
            return AddHostWeb(model, definition, null);
        }

        public static SiteModelNode AddHostWeb(this SiteModelNode model, WebDefinition definition, Action<WebModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        public static WebModelNode AddHostWeb(this WebModelNode model, WebDefinition definition)
        {
            return AddHostWeb(model, definition, null);
        }

        public static WebModelNode AddHostWeb(this WebModelNode model, WebDefinition definition, Action<WebModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
