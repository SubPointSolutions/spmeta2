using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class WelcomePageDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWelcomePage(this ModelNode model, WelcomePageDefinition definition)
        {
            return AddWelcomePage(model, definition, null);
        }

        public static ModelNode AddWelcomePage(this ModelNode model, WelcomePageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
