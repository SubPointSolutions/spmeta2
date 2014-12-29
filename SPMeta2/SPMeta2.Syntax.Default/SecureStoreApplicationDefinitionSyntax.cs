using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SecureStoreApplicationDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecureStoreApplication(this ModelNode model, SecureStoreApplicationDefinition definition)
        {
            return AddSecureStoreApplication(model, definition, null);
        }

        public static ModelNode AddSecureStoreApplication(this ModelNode model, SecureStoreApplicationDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region add host

        public static ModelNode AddHostSecureStoreApplication(this ModelNode model, SecureStoreApplicationDefinition definition)
        {
            return AddHostSecureStoreApplication(model, definition, null);
        }

        public static ModelNode AddHostSecureStoreApplication(this ModelNode model, SecureStoreApplicationDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
