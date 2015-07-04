using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class PrefixModelNode : TypedModelNode
    {

    }

    public static class PrefixDefinitionSyntax
    {
        #region methods

        public static WebApplicationModelNode AddPrefix(this WebApplicationModelNode model, PrefixDefinition definition)
        {
            return AddPrefix(model, definition, null);
        }

        public static WebApplicationModelNode AddPrefix(this WebApplicationModelNode model, PrefixDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
