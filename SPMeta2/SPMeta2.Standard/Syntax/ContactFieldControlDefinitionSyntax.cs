using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class ContactFieldControlDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddContactFieldControl(this ModelNode model, ContactFieldControlDefinition definition)
        {
            return AddContactFieldControl(model, definition, null);
        }

        public static ModelNode AddContactFieldControl(this ModelNode model, ContactFieldControlDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddContactFieldControls(this ModelNode model, IEnumerable<ContactFieldControlDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
