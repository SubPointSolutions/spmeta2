using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

using SPMeta2.Utils;
using System.Linq;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Services;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListViewDefinitionValidator : ListViewModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            var context = list.Context;

            context.Load(list, l => l.Fields);
            context.Load(list, l => l.Views.Include(
                v => v.ViewFields,
                v => v.Title,
                v => v.DefaultView,
                v => v.ViewQuery,
                v => v.RowLimit,
                v => v.Paged,
                v => v.Scope,
                v => v.Hidden,
                v => v.JSLink,
                v => v.ServerRelativeUrl,
                v => v.DefaultViewForContentType,
                v => v.ContentTypeId,
                v => v.AggregationsStatus,
                v => v.Aggregations,
                v => v.ViewType,
                v => v.ViewData));
            context.ExecuteQueryWithTrace();

            var spObject = FindViewByTitle(list.Views, definition.Title);
            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                .ShouldBeEqual(m => m.IsDefault, o => o.DefaultView)
                .ShouldBeEqual(m => m.Hidden, o => o.Hidden)
                .ShouldBeEqual(m => m.RowLimit, o => (int)o.RowLimit)
                .ShouldBeEqual(m => m.IsPaged, o => o.Paged);

            if (!string.IsNullOrEmpty(definition.Scope))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Scope);
                    var dstProp = d.GetExpressionValue(o => o.Scope);

                    var scopeValue = ListViewScopeTypesConvertService.NormilizeValueToCSOMType(definition.Scope);

                    var isValid = scopeValue == d.Scope.ToString();

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
            {
                assert.SkipProperty(m => m.Scope);
            }

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
                    var dstProp = d.GetExpressionValue(o => o.ViewType);

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

            assert.SkipProperty(m => m.ViewStyleId, "ViewStyleId unsupported by SP CSOM API yet. Skipping.");
            assert.SkipProperty(m => m.TabularView, "TabularView unsupported by SP CSOM API yet. Skipping.");
            assert.SkipProperty(m => m.InlineEdit, "InlineEdit unsupported by SP CSOM API yet. Skipping.");

            assert.ShouldBeEqualIfNotNullOrEmpty(m => m.JSLink, o => o.JSLink);

            if (!string.IsNullOrEmpty(definition.Query))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Query);
                    var dstProp = d.GetExpressionValue(o => o.ViewQuery);

                    var srcViewDate = assert.Src.Query.Replace(System.Environment.NewLine, string.Empty).Replace(" /", "/");
                    var dstViewDate = assert.Dst.ViewQuery.Replace(System.Environment.NewLine, string.Empty).Replace(" /", "/");

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
                assert.SkipProperty(m => m.Query, "Query is null or empty. Skipping.");

            assert.ShouldBeEqualIfHasValue(m => m.DefaultViewForContentType, o => o.DefaultViewForContentType);

            if (string.IsNullOrEmpty(definition.ContentTypeName))
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is null or empty. Skipping.");
            else
            {
                var contentTypeId = LookupListContentTypeByName(list, definition.ContentTypeName);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeName);
                    var dstProp = d.GetExpressionValue(ct => ct.ContentTypeId);

                    var isValis = contentTypeId.StringValue == d.ContentTypeId.StringValue;

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

                    var isValis = contentTypeId.StringValue == d.ContentTypeId.StringValue;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValis
                    };
                });
            }

            if (string.IsNullOrEmpty(definition.AggregationsStatus))
                assert.SkipProperty(m => m.AggregationsStatus, "Aggregationsstatus is null or empty. Skipping.");
            else
                assert.ShouldBeEqual(m => m.AggregationsStatus, o => o.AggregationsStatus);

            if (string.IsNullOrEmpty(definition.Aggregations))
                assert.SkipProperty(m => m.Aggregations, "Aggregations is null or empty. Skipping.");
            else
                assert.ShouldBeEqual(m => m.Aggregations, o => o.Aggregations);

            assert.ShouldBePartOfIfNotNullOrEmpty(m => m.Url, o => o.ServerRelativeUrl);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Fields);
                var dstProp = d.GetExpressionValue(ct => ct.ViewFields);

                var hasAllFields = true;

                foreach (var srcField in s.Fields)
                {
                    var listField = list.Fields.ToList().FirstOrDefault(f => f.StaticName == srcField);

                    // if list-scoped field we need to check by internal name
                    // internal name is changed for list scoped-fields
                    // that's why to check by BOTH, definition AND real internal name

                    if (!d.ViewFields.ToList().Contains(srcField) &&
                         !d.ViewFields.ToList().Contains(listField.InternalName))
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

            var supportsLocalization = ReflectionUtils.HasProperties(spObject, new[]
            {
                "TitleResource"
            });

            if (supportsLocalization)
            {
                if (definition.TitleResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.TitleResource);
                        var isValid = true;

                        foreach (var userResource in s.TitleResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "TitleResource");

                            var value = ReflectionUtils.GetMethod(resourceObject, "GetValueForUICulture")
                                                    .Invoke(resourceObject, new[] { culture.Name }) as ClientResult<string>;

                            context.ExecuteQuery();

                            isValid = userResource.Value == value.Value;

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
            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      "CSOM runtime doesn't have Web.TitleResource and Web.DescriptionResource() methods support. Skipping validation.");

                assert.SkipProperty(m => m.TitleResource, "TitleResource is null or empty. Skipping.");
            }
        }

        protected bool DoesFieldExist(ViewFieldCollection viewFields, string fieldName)
        {
            foreach (var viewField in viewFields)
            {
                if (System.String.Compare(viewField, fieldName, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }

            return false;
        }
    }
}
