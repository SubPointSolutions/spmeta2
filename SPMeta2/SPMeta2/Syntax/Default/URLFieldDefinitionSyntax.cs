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
    public static class URLFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddURLField(this ModelNode model, URLFieldDefinition definition)
        {
            return AddURLField(model, definition, null);
        }

        public static ModelNode AddURLField(this ModelNode model, URLFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddURLFields(this ModelNode model, IEnumerable<URLFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
