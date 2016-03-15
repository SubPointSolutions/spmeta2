using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Extensions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientComposedLookItemDefinitionValidator : ClientListItemDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ComposedLookItemDefinition); }
        }

        protected override void ValidateProperties(ListItem item, ListItemDefinition definition)
        {
            base.ValidateProperties(item, definition);

            var typedDefinition = definition as ComposedLookItemDefinition;

            var assert = ServiceFactory.AssertService.NewAssert(typedDefinition, item);


            assert
                .ShouldNotBeNull(item)

                .ShouldBeEqual(m => m.Name, o => o.GetComposedLookName())

                .ShouldBeEqualIfHasValue(m => m.DisplayOrder, o => o.GetComposedLookDisplayOrder());

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
    }
}
