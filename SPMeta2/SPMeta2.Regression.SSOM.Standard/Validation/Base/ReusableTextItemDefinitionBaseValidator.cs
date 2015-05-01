using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Base
{
    public class ReusableTextItemDefinitionBaseValidator : ListItemDefinitionValidator
    {
        protected override void ValidateProperties(SPListItem item, ListItemDefinition definition)
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
        public static string GetContentCategory(this SPListItem item)
        {
            return ConvertUtils.ToString(item["ContentCategory"]);
        }

        public static string GetReusableHTML(this SPListItem item)
        {
            return ConvertUtils.ToString(item["ReusableHTML"]);
        }

        public static string GetReusableText(this SPListItem item)
        {
            return ConvertUtils.ToString(item["ReusableText"]);
        }

        public static string GetComments(this SPListItem item)
        {
            return ConvertUtils.ToString(item["Comments"]);
        }

        public static bool? GetAutomaticUpdate(this SPListItem item)
        {
            return ConvertUtils.ToBool(item["AutomaticUpdate"]);
        }

        public static bool? GetShowInRibbon(this SPListItem item)
        {
            return ConvertUtils.ToBool(item["ShowInRibbon"]);
        }
    }
}
