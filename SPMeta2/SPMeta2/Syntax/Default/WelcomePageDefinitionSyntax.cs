using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class WelcomePageDefinitionSyntax
    {
        #region methods

        public static WebModelNode AddWelcomePage(this WebModelNode model, WelcomePageDefinition definition)
        {
            return AddWelcomePage(model, definition, null);
        }

        public static WebModelNode AddWelcomePage(this WebModelNode model, WelcomePageDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }


        public static ListModelNode AddWelcomePage(this ListModelNode model, WelcomePageDefinition definition)
        {
            return AddWelcomePage(model, definition, null);
        }

        public static ListModelNode AddWelcomePage(this ListModelNode model, WelcomePageDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }


        public static FolderModelNode AddWelcomePage(this FolderModelNode model, WelcomePageDefinition definition)
        {
            return AddWelcomePage(model, definition, null);
        }

        public static FolderModelNode AddWelcomePage(this FolderModelNode model, WelcomePageDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }





        #endregion
    }
}
