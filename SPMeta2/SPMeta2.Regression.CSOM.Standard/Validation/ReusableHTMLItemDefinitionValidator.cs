using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Standard.Validation.Base;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Base;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ReusableHTMLItemDefinitionValidator : ReusableTextItemDefinitionBaseValidator
    {
        public override Type TargetType
        {
            get { return typeof(ReusableHTMLItemDefinition); }
        }

        protected override void ValidateProperties(ListItem item, ListItemDefinition definition)
        {
            base.ValidateProperties(item, definition);

            var typedDefinition = definition as ReusableHTMLItemDefinition;

            var assert = ServiceFactory.AssertService.NewAssert(typedDefinition, item);

            assert
                .ShouldNotBeNull(item)
                .ShouldBeEqualIfNotNullOrEmpty(m => m.ReusableHTML, o => o.GetReusableHTML());
        }
    }
}
