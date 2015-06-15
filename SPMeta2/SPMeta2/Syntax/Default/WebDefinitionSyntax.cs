using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class WebModelNode : TypedModelNode
    {



    }

    public static class WebDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddWeb(this SiteModelNode model, WebDefinition definition)
        {
            return AddWeb(model, definition, null);
        }

        public static SiteModelNode AddWeb(this SiteModelNode model, WebDefinition definition, Action<WebModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddWeb(this WebModelNode model, WebDefinition definition)
        {
            return AddWeb(model, definition, null);
        }

        public static WebModelNode AddWeb(this WebModelNode model, WebDefinition definition, Action<WebModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static WebModelNode AddWebs(this WebModelNode model, IEnumerable<WebDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        public static SiteModelNode AddWebs(this SiteModelNode model, IEnumerable<WebDefinition> definitions)
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
