﻿using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListViewDefinitionValidator : ListViewModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var spObject = list.Views.FindByName(definition.Title);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                               .ShouldBeEqual(m => m.Title, o => o.Title)
                               .ShouldBeEqual(m => m.IsDefault, o => o.IsDefaul())
                               .ShouldBeEqual(m => m.Hidden, o => o.Hidden)
                               .ShouldBeEqual(m => m.RowLimit, o => (int)o.RowLimit)
                               .ShouldBeEqual(m => m.IsPaged, o => o.Paged);

            if (!string.IsNullOrEmpty(definition.Query))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Query);
                    var dstProp = d.GetExpressionValue(o => o.Query);

                    var srcViewDate = assert.Src.Query.Replace(System.Environment.NewLine, string.Empty).Replace(" /", "/");
                    var dstViewDate = assert.Dst.Query.Replace(System.Environment.NewLine, string.Empty).Replace(" /", "/");

                    var isValid = srcViewDate.ToUpper() == dstViewDate.ToUpper();

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
                assert.SkipProperty(m => m.Query);


            if (!string.IsNullOrEmpty(definition.ViewData))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ViewData);
                    var dstProp = d.GetExpressionValue(o => o.ViewData);

                    var srcViewDate = assert.Src.ViewData.Replace(System.Environment.NewLine, string.Empty).Replace(" /", "/");
                    var dstViewDate = assert.Dst.ViewData.Replace(System.Environment.NewLine, string.Empty).Replace(" /", "/");

                    var isValid = srcViewDate.ToUpper() == dstViewDate.ToUpper();

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
                assert.SkipProperty(m => m.ViewData);

            if (!string.IsNullOrEmpty(definition.Type))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Type);
                    var dstProp = d.GetExpressionValue(o => o.Type);

                    var isValid = srcProp.Value.ToString().ToUpper() ==
                        dstProp.Value.ToString().ToUpper();

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
                assert.SkipProperty(m => m.Type);

            if (!string.IsNullOrEmpty(definition.JSLink))
                assert.ShouldBeEqual(m => m.JSLink, o => o.JSLink);
            else
                assert.SkipProperty(m => m.JSLink);

            if (definition.DefaultViewForContentType.HasValue)
                assert.ShouldBeEqual(m => m.DefaultViewForContentType, o => o.DefaultViewForContentType);
            else
                assert.SkipProperty(m => m.DefaultViewForContentType, "DefaultViewForContentType is null or empty. Skipping.");

            if (string.IsNullOrEmpty(definition.ContentTypeName))
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is null or empty. Skipping.");
            else
            {
                var contentTypeId = LookupListContentTypeByName(list, definition.ContentTypeName);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeName);
                    var dstProp = d.GetExpressionValue(ct => ct.ContentTypeId);

                    var isValis = contentTypeId == d.ContentTypeId;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValis
                    };
                });
            }

            if (string.IsNullOrEmpty(definition.ContentTypeId))
                assert.SkipProperty(m => m.ContentTypeId, "ContentTypeId is null or empty. Skipping.");
            else
            {
                var contentTypeId = LookupListContentTypeById(list, definition.ContentTypeId);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeId);
                    var dstProp = d.GetExpressionValue(ct => ct.ContentTypeId);

                    var isValis = contentTypeId == d.ContentTypeId;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValis
                    };
                });
            }

            if (string.IsNullOrEmpty(definition.Url))
                assert.SkipProperty(m => m.Url, "Url is null or empty. Skipping.");
            else
                assert.ShouldBePartOf(m => m.Url, o => o.ServerRelativeUrl);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Fields);
                var dstProp = d.GetExpressionValue(ct => ct.ViewFields);

                var hasAllFields = true;

                foreach (var srcField in s.Fields)
                {
                    var listField = d.ParentList.Fields.OfType<SPField>().FirstOrDefault(f => f.StaticName == srcField);

                    // if list-scoped field we need to check by internal name
                    // internal name is changed for list scoped-fields
                    // that's why to check by BOTH, definition AND real internal name

                    if (!d.ViewFields.ToStringCollection().Contains(srcField) &&
                        !d.ViewFields.ToStringCollection().Contains(listField.InternalName))
                        hasAllFields = false;
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = hasAllFields
                };
            });

            /// localization
            if (definition.TitleResource.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.TitleResource);
                    var isValid = true;

                    foreach (var userResource in s.TitleResource)
                    {
                        var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                        var value = d.TitleResource.GetValueForUICulture(culture);

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
                assert.SkipProperty(m => m.TitleResource, "TitleResource is NULL or empty. Skipping.");
            }

        }
    }

    internal static class ViewDefault
    {
        public static string GetType(this ListViewDefinition def)
        {
            return def.Type.ToUpper();
        }

        public static string GetType(this SPView view)
        {
            return view.Type.ToUpper();
        }

        public static bool IsDefaul(this SPView view)
        {
            return view.ParentList.DefaultView.ID == view.ID;
        }
    }
}
