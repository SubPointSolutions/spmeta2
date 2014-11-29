using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Webparts;
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
    public static class ScriptEditorWebPartDefinitionSyntax
    {
        #region methods

        public static ModelNode AddScriptEditorWebPart(this ModelNode model, ScriptEditorWebPartDefinition definition)
        {
            return AddScriptEditorWebPart(model, definition, null);
        }

        public static ModelNode AddScriptEditorWebPart(this ModelNode model, ScriptEditorWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
