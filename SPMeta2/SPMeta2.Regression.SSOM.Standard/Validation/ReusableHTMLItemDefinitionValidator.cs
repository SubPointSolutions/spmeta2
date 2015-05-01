using System;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Standard.Validation.Base;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class ReusableHTMLItemDefinitionValidator : ReusableTextItemDefinitionBaseValidator
    {
        public override Type TargetType
        {
            get { return typeof(ReusableHTMLItemDefinition); }
        }

        protected override void ValidateProperties(SPListItem item, ListItemDefinition definition)
        {
            base.ValidateProperties(item, definition);

            var typedDefinition = definition as ReusableHTMLItemDefinition;
            var assert = ServiceFactory.AssertService
                             .NewAssert(typedDefinition, item)
                              .ShouldNotBeNull(item);

            if (!string.IsNullOrEmpty(typedDefinition.ReusableHTML))
                assert.SkipProperty(m => m.ReusableHTML);
            else
                assert.ShouldBeEqual(m => m.ReusableHTML, o => o.GetReusableText());
        }
    }
}
