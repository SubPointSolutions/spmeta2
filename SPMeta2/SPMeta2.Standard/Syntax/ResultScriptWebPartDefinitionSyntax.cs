using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class ResultScriptWebPartDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddResultScriptWebPart(this ModelNode model, ResultScriptWebPartDefinition definition)
        {
            return AddResultScriptWebPart(model, definition, null);
        }

        public static ModelNode AddResultScriptWebPart(this ModelNode model, ResultScriptWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddResultScriptWebParts(this ModelNode model, IEnumerable<ResultScriptWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
