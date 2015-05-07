using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Base;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientComposedLookItemDefinitionValidator : ClientListItemDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof (ComposedLookItemDefinition); }
        }

        protected override void ValidateProperties(ListItem item, ListItemDefinition definition)
        {
            base.ValidateProperties(item, definition);

            var typedDefinition = definition as ComposedLookItemDefinition;
            var assert = ServiceFactory.AssertService
                             .NewAssert(typedDefinition, item)
                              .ShouldNotBeNull(item);

           // TODO
        }
    }

}
