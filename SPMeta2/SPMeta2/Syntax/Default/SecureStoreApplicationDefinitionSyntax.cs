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
    public class SecureStoreApplicationModelNode : TypedModelNode
    {

    }

    public static class SecureStoreApplicationDefinitionSyntax
    {
        #region methods

        public static FarmModelNode AddSecureStoreApplication(this FarmModelNode model, SecureStoreApplicationDefinition definition)
        {
            return AddSecureStoreApplication(model, definition, null);
        }

        public static FarmModelNode AddSecureStoreApplication(this FarmModelNode model, SecureStoreApplicationDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
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
