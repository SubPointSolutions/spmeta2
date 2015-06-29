using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public interface IWelcomePageHostModelNode
    {

    }

    public static class WelcomePageDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWelcomePage<TModelNode>(this TModelNode model, WelcomePageDefinition definition)
            where TModelNode : ModelNode, IWelcomePageHostModelNode, new()
        {
            return AddWelcomePage(model, definition, null);
        }

        public static TModelNode AddWelcomePage<TModelNode>(this TModelNode model, WelcomePageDefinition definition,
            Action<FieldModelNode> action)
            where TModelNode : ModelNode, IWelcomePageHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
