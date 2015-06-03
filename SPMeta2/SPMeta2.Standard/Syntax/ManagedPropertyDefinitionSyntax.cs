using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class ManagedPropertyDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddManagedProperty(this ModelNode model, ManagedPropertyDefinition definition)
        {
            return AddManagedProperty(model, definition, null);
        }

        public static ModelNode AddManagedProperty(this ModelNode model, ManagedPropertyDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddManagedProperties(this ModelNode model, IEnumerable<ManagedPropertyDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
