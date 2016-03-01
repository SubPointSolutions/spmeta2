using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

using System;
using System.Linq;
using SPMeta2.Regression.SSOM.Extensions;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentTypeDefinitionValidator : ContentTypeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var web = ExtractWeb(modelHost);
            var definition = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var site = web.Site;

            var contentTypes = web.AvailableContentTypes;
            var spObject = contentTypes[definition.Name];

            if (spObject == null)
                spObject = web.ContentTypes[definition.Name];

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Name, o => o.Name)
                .ShouldBeEqual(m => m.Group, o => o.Group)
                .ShouldBeEqual(m => m.Hidden, o => o.Hidden);
            //.ShouldBeEqual(m => m.Description, o => o.Description);


            if (definition.Sealed.HasValue)
                assert.ShouldBeEqual(m => m.Sealed, o => o.Sealed);
            else
                assert.SkipProperty(m => m.Sealed, "Sealed is null or empty. Skipping.");

            if (definition.ReadOnly.HasValue)
                assert.ShouldBeEqual(m => m.ReadOnly, o => o.ReadOnly);
            else
                assert.SkipProperty(m => m.ReadOnly, "ReadOnly is null or empty. Skipping.");


            if (!string.IsNullOrEmpty(definition.JSLink))
                assert.ShouldBeEqual(m => m.JSLink, o => o.JSLink);
            else
                assert.SkipProperty(m => m.JSLink, "JSLink is null or empty. Skipping.");

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description, "Description is null or empty. Skipping.");

            if (definition.Id == default(Guid))
            {
                assert.SkipProperty(m => m.IdNumberValue, string.Format("Skipping Id as it is default(Guid)"));
            }
            else
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Id);
                    var dstProp = d.GetExpressionValue(ct => ct.GetId());

                    var srcCtId = s.GetContentTypeId();
                    var dstCtId = d.GetId();

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = dstCtId.ToString().ToUpper() == dstCtId.ToString().ToUpper()
                    };
                });
            }

            if (string.IsNullOrEmpty(definition.IdNumberValue))
            {
                assert.SkipProperty(m => m.IdNumberValue, string.Format("Skipping IdNumberValue as it is Empty"));
            }
            else
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Id);
                    var dstProp = d.GetExpressionValue(ct => ct.GetId());

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = srcProp.ToString().ToUpper() == dstProp.ToString().ToUpper()
                    };
                });
            }

            if (string.IsNullOrEmpty(definition.DocumentTemplate))
            {
                assert.SkipProperty(m => m.DocumentTemplate, string.Format("Skipping DocumentTemplate as it is Empty"));
            }
            else
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.DocumentTemplate);
                    var dstProp = d.GetExpressionValue(ct => ct.DocumentTemplateUrl);

                    var srcUrl = srcProp.Value as string;
                    var dstUrl = dstProp.Value as string;

                    var isValid = false;

                    if (s.DocumentTemplate.Contains("~sitecollection"))
                    {
                        var siteCollectionUrl = web.Site.ServerRelativeUrl == "/" ? string.Empty : web.Site.ServerRelativeUrl;

                        isValid = srcUrl.Replace("~sitecollection", siteCollectionUrl) == dstUrl;
                    }
                    else if (s.DocumentTemplate.Contains("~site"))
                    {
                        var siteCollectionUrl = web.ServerRelativeUrl == "/" ? string.Empty : web.ServerRelativeUrl;

                        isValid = srcUrl.Replace("~site", siteCollectionUrl) == dstUrl;
                    }
                    else
                    {
                        isValid = dstUrl.EndsWith(srcUrl);
                    }

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }

            /// localization
            if (definition.NameResource.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.NameResource);
                    var isValid = true;

                    foreach (var userResource in s.NameResource)
                    {
                        var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                        var value = d.NameResource.GetValueForUICulture(culture);

                        isValid = userResource.Value == value;

                        if (!isValid)
                            break;
                    }

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
                assert.SkipProperty(m => m.NameResource, "NameResource is NULL or empty. Skipping.");
            }

            if (definition.DescriptionResource.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.DescriptionResource);
                    var isValid = true;

                    foreach (var userResource in s.DescriptionResource)
                    {
                        var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                        var value = d.DescriptionResource.GetValueForUICulture(culture);

                        isValid = userResource.Value == value;

                        if (!isValid)
                            break;
                    }

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
                assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is NULL or empty. Skipping.");
            }
        }
    }
}
