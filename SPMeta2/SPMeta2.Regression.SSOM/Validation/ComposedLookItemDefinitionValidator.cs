using System;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ComposedLookItemDefinitionValidator : ListItemDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ComposedLookItemDefinition); }
        }

        #region methods

        protected override void ValidateProperties(SPListItem item, ListItemDefinition definition)
        {
            base.ValidateProperties(item, definition);

            var typedDefinition = definition as ComposedLookItemDefinition;

            var assert = ServiceFactory.AssertService
                        .NewAssert(typedDefinition, item)
                        .ShouldNotBeNull(item)
                        .ShouldBeEqual(m => m.Name, o => o.GetComposedLookName());

            if (typedDefinition.DisplayOrder.HasValue)
                assert.ShouldBeEqual(m => m.DisplayOrder, o => o.GetComposedLookDisplayOrder());
            else
                assert.SkipProperty(m => m.DisplayOrder, "DisplayOrder is NULL");

            // master page
            if (!string.IsNullOrEmpty(typedDefinition.MasterPageUrl))
            {
                var urlValue = item.GetComposedLookMasterPageUrl();

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.MasterPageUrl);
                    var isValid = (urlValue != null) && (urlValue.Url == s.MasterPageUrl);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.MasterPageDescription);
                    var isValid = (urlValue != null) && (urlValue.Description == s.MasterPageDescription);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.MasterPageUrl, "MasterPageUrl is NULL");
                assert.SkipProperty(m => m.MasterPageDescription, "MasterPageDescription is NULL");
            }

            // font scheme
            if (!string.IsNullOrEmpty(typedDefinition.FontSchemeUrl))
            {
                var urlValue = item.GetComposedLookFontSchemeUrl();

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.FontSchemeUrl);
                    var isValid = (urlValue != null) && (urlValue.Url == s.FontSchemeUrl);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.FontSchemeDescription);
                    var isValid = (urlValue != null) && (urlValue.Description == s.FontSchemeDescription);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.FontSchemeUrl, "MasterPageUrl is NULL");
                assert.SkipProperty(m => m.FontSchemeDescription, "MasterPageDescription is NULL");
            }

            // image url
            if (!string.IsNullOrEmpty(typedDefinition.ImageUrl))
            {
                var urlValue = item.GetComposedLookImageUrl();

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.ImageUrl);
                    var isValid = (urlValue != null) && (urlValue.Url == s.ImageUrl);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.ImageDescription);
                    var isValid = (urlValue != null) && (urlValue.Description == s.ImageDescription);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.ImageUrl, "ImageUrl is NULL");
                assert.SkipProperty(m => m.ImageDescription, "ImageDescription is NULL");
            }

            // theme url
            if (!string.IsNullOrEmpty(typedDefinition.ThemeUrl))
            {
                var urlValue = item.GetComposedLookThemeUrl();

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.ThemeUrl);
                    var isValid = (urlValue != null) && (urlValue.Url == s.ThemeUrl);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.ThemeDescription);
                    var isValid = (urlValue != null) && (urlValue.Description == s.ThemeDescription);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.ThemeUrl, "ThemeUrl is NULL");
                assert.SkipProperty(m => m.ThemeDescription, "ThemeDescription is NULL");
            }
        }

        #endregion
    }

    internal static class ComposedLookItemDefinitionHelpers
    {

        public static string GetComposedLookName(this SPListItem item)
        {
            return item["Name"] as string;
        }

        public static SPFieldUrlValue GetComposedLookMasterPageUrl(this SPListItem item)
        {
            return ConvertToSPFieldUrlValue(item["MasterPageUrl"]);
        }

        private static SPFieldUrlValue ConvertToSPFieldUrlValue(object value)
        {
            var stringValue = ConvertUtils.ToString(value);

            if (!string.IsNullOrEmpty(stringValue))
                return new SPFieldUrlValue(stringValue);

            return null;
        }

        public static SPFieldUrlValue GetComposedLookThemeUrl(this SPListItem item)
        {
            return ConvertToSPFieldUrlValue(item["ThemeUrl"]);
        }

        public static SPFieldUrlValue GetComposedLookImageUrl(this SPListItem item)
        {
            return ConvertToSPFieldUrlValue(item["ImageUrl"]);
        }

        public static SPFieldUrlValue GetComposedLookFontSchemeUrl(this SPListItem item)
        {
            return ConvertToSPFieldUrlValue(item["FontSchemeUrl"]);
        }

        public static int? GetComposedLookDisplayOrder(this SPListItem item)
        {
            return ConvertUtils.ToInt(item["DisplayOrder"]);
        }

    }
}
