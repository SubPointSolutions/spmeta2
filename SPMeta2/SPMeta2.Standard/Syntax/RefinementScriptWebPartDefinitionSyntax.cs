using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class RefinementScriptWebPartDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddRefinementScriptWebPart(this ModelNode model, RefinementScriptWebPartDefinition definition)
        {
            return AddRefinementScriptWebPart(model, definition, null);
        }

        public static ModelNode AddRefinementScriptWebPart(this ModelNode model, RefinementScriptWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddRefinementScriptWebParts(this ModelNode model, IEnumerable<RefinementScriptWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
