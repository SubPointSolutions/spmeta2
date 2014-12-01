using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
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
    public static class BusineddDataFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddBusinessDataField(this ModelNode model, BusinessDataFieldDefinition definition)
        {
            return AddBusinessDataField(model, definition, null);
        }

        public static ModelNode AddBusinessDataField(this ModelNode model, BusinessDataFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

       
    }
}
