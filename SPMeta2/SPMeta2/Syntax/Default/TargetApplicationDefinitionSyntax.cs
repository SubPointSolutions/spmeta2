using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class TargetApplicationModelNode : TypedModelNode
    {

    }

    public static class TargetApplicationDefinitionSyntax
    {
        #region methods

        public static SecureStoreApplicationModelNode AddTargetApplication(this SecureStoreApplicationModelNode model, TargetApplicationDefinition definition)
        {
            return AddTargetApplication(model, definition, null);
        }

        public static SecureStoreApplicationModelNode AddTargetApplication(this SecureStoreApplicationModelNode model, TargetApplicationDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
