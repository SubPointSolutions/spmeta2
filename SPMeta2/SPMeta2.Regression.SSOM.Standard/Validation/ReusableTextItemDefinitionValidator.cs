using System;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Standard.Validation.Base;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class ReusableTextItemDefinitionValidator : ReusableTextItemDefinitionBaseValidator
    {
        public override Type TargetType
        {
            get { return typeof(ReusableTextItemDefinition); }
        }

        protected override void ValidateProperties(SPListItem item, ListItemDefinition definition)
        {
            base.ValidateProperties(item, definition);

            var typedDefinition = definition as ReusableTextItemDefinition;
            var assert = ServiceFactory.AssertService
                             .NewAssert(typedDefinition, item)
                              .ShouldNotBeNull(item);

            if (!string.IsNullOrEmpty(typedDefinition.ReusableText))
                assert.SkipProperty(m => m.ReusableText);
            else
                assert.ShouldBeEqual(m => m.Title, o => o.GetReusableText());
        }
    }
}

