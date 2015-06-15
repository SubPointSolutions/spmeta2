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
    public static class DependentLookupFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddDependentLookupField(this ModelNode model, DependentLookupFieldDefinition definition)
        {
            return AddDependentLookupField(model, definition, null);
        }

        public static ModelNode AddDependentLookupField(this ModelNode model, DependentLookupFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddDependentLookupFields(this ModelNode model, IEnumerable<DependentLookupFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
