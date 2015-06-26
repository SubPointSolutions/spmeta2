using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class RootWebModelNode : TypedModelNode
    {

    }

    public static class RootWebDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddRootWeb(this SiteModelNode model, RootWebDefinition definition)
        {
            return AddRootWeb(model, definition, null);
        }

        public static SiteModelNode AddRootWeb(this SiteModelNode model, RootWebDefinition definition,
            Action<RootWebModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddRootWeb(this WebModelNode model, RootWebDefinition definition)
        {
            return AddRootWeb(model, definition, null);
        }

        public static WebModelNode AddRootWeb(this WebModelNode model, RootWebDefinition definition,
            Action<RootWebModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region host override

        public static ModelNode AddHostRootWeb(this ModelNode model, RootWebDefinition definition)
        {
            return AddHostRootWeb(model, definition, null);
        }

        public static ModelNode AddHostRootWeb(this ModelNode model, RootWebDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
