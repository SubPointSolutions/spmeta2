using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Base
{
    public class ReusableTextItemDefinitionBaseValidator : ClientListItemDefinitionValidator
    {
        protected override void ValidateProperties(ListItem item, ListItemDefinition definition)
        {
            base.ValidateProperties(item, definition);

            var typedDefinition = definition as ReusableItemDefinitionBase;
            var assert = ServiceFactory.AssertService
                             .NewAssert(typedDefinition, item)
                              .ShouldNotBeNull(item);

            if (!string.IsNullOrEmpty(typedDefinition.ContentCategory))
                assert.ShouldBeEqual(m => m.ContentCategory, o => o.GetContentCategory());
            else
                assert.SkipProperty(m => m.ContentCategory);

            assert
               .ShouldBeEqual(m => m.Comments, o => o.GetComments())
               .ShouldBeEqual(m => m.AutomaticUpdate, o => o.GetAutomaticUpdate())
               .ShouldBeEqual(m => m.ShowInDropDownMenu, o => o.GetShowInRibbon());
        }
    }

    internal static class ReusableTextItemDefinitionBaseValidatorUtils
    {
        public static string GetContentCategory(this ListItem item)
        {
            return ConvertUtils.ToString(item["ContentCategory"]);
        }

        public static string GetReusableHTML(this ListItem item)
        {
            return ConvertUtils.ToString(item["ReusableHTML"]);
        }

        public static string GetReusableText(this ListItem item)
        {
            return ConvertUtils.ToString(item["ReusableText"]);
        }

        public static string GetComments(this ListItem item)
        {
            return ConvertUtils.ToString(item["Comments"]);
        }

        public static bool? GetAutomaticUpdate(this ListItem item)
        {
            return ConvertUtils.ToBool(item["AutomaticUpdate"]);
        }

        public static bool? GetShowInRibbon(this ListItem item)
        {
            return ConvertUtils.ToBool(item["ShowInRibbon"]);
        }
    }
}
