using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class MasterPageSettingsModelNode : TypedModelNode
    {

    }

    public static class MasterPageSettingsDefinitionSyntax
    {
        #region methods

        public static WebModelNode AddMasterPageSettings(this WebModelNode model, MasterPageSettingsDefinition definition)
        {
            return AddMasterPageSettings(model, definition, null);
        }

        public static WebModelNode AddMasterPageSettings(this WebModelNode model, MasterPageSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion


    }
}
