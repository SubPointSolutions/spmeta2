using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SilverlightWebPartDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSilverlightWebPart(this ModelNode model, SilverlightWebPartDefinition definition)
        {
            return AddSilverlightWebPart(model, definition, null);
        }

        public static ModelNode AddSilverlightWebPart(this ModelNode model, SilverlightWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddSilverlightWebParts(this ModelNode model, IEnumerable<SilverlightWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
