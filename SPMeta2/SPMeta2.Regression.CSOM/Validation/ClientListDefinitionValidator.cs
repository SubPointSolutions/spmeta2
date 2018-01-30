using System;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListDefinitionValidator : ListModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;
            var context = web.Context;

            context.Load(web, w => w.ServerRelativeUrl);

            var lists = context.LoadQuery<List>(web.Lists.Include(l => l.DefaultViewUrl));
            context.ExecuteQueryWithTrace();

#pragma warning disable 618
            var spObject = FindListByUrl(lists, definition.GetListUrl());
#pragma warning restore 618

            context.Load(spObject);
            context.Load(spObject, list => list.RootFolder.Properties);
            context.Load(spObject, list => list.RootFolder.ServerRelativeUrl);
            context.Load(spObject, list => list.RootFolder.Properties);
            context.Load(spObject, list => list.EnableAttachments);
            context.Load(spObject, list => list.EnableFolderCreation);
            context.Load(spObject, list => list.EnableMinorVersions);
            context.Load(spObject, list => list.EnableModeration);
            context.Load(spObject, list => list.EnableVersioning);
            context.Load(spObject, list => list.ForceCheckout);
            context.Load(spObject, list => list.Hidden);
            context.Load(spObject, list => list.NoCrawl);
            context.Load(spObject, list => list.OnQuickLaunch);
            context.Load(spObject, list => list.DocumentTemplateUrl);
            context.Load(spObject, list => list.DraftVersionVisibility);

            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.Title, o => o.Title)
                //.ShouldBeEqual(m => m.Description, o => o.Description)
                //.ShouldBeEqual(m => m.IrmEnabled, o => o.IrmEnabled)
                //.ShouldBeEqual(m => m.IrmExpire, o => o.IrmExpire)
                //.ShouldBeEqual(m => m.IrmReject, o => o.IrmReject)
                //.ShouldBeEndOf(m => m.GetServerRelativeUrl(web), m => m.Url, o => o.GetServerRelativeUrl(), o => o.GetServerRelativeUrl())
                .ShouldBeEqual(m => m.ContentTypesEnabled, o => o.ContentTypesEnabled);


            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description, "Description is null or empty. Skipping.");

            assert.SkipProperty(m => m.EnableAssignToEmail, "EnableAssignToEmail is not supported by CSOM");
            assert.SkipProperty(m => m.DisableGridEditing, "DisableGridEditing is not supported by CSOM");
            assert.SkipProperty(m => m.WriteSecurity, "WriteSecurity is not supported by CSOM");
            assert.SkipProperty(m => m.NavigateForFormsPages, "NavigateForFormsPages is not supported by CSOM");

            if (!string.IsNullOrEmpty(definition.DraftVersionVisibility))
            {
                var draftOption = (DraftVisibilityType)Enum.Parse(typeof(DraftVisibilityType), definition.DraftVersionVisibility);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.DraftVersionVisibility);
                    var dstProp = d.GetExpressionValue(m => m.DraftVersionVisibility);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = draftOption == (DraftVisibilityType)dstProp.Value
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.DraftVersionVisibility, "Skipping from validation. DraftVersionVisibility IS NULL");
            }

            if (definition.Hidden.HasValue)
                assert.ShouldBeEqual(m => m.Hidden, m => m.Hidden);
            else
                assert.SkipProperty(m => m.Hidden, "Skipping from validation. Url IS NULL");

#pragma warning disable 618
            if (!string.IsNullOrEmpty(definition.Url))
                assert.ShouldBeEndOf(m => m.GetListUrl(), m => m.Url, o => o.GetServerRelativeUrl(), o => o.GetServerRelativeUrl());
            else
                assert.SkipProperty(m => m.Url, "Skipping from validation. Url IS NULL");
#pragma warning restore 618

            if (!string.IsNullOrEmpty(definition.CustomUrl))
                assert.ShouldBeEndOf(m => m.CustomUrl, o => o.GetServerRelativeUrl());
            else
                assert.SkipProperty(m => m.CustomUrl, "Skipping from validation. CustomUrl IS NULL");

            // common
            if (definition.EnableAttachments.HasValue)
                assert.ShouldBeEqual(m => m.EnableAttachments, o => o.EnableAttachments);
            else
                assert.SkipProperty(m => m.EnableAttachments, "Skipping from validation. EnableAttachments IS NULL");

            if (definition.EnableFolderCreation.HasValue)
                assert.ShouldBeEqual(m => m.EnableFolderCreation, o => o.EnableFolderCreation);
            else
                assert.SkipProperty(m => m.EnableFolderCreation, "Skipping from validation. EnableFolderCreation IS NULL");

            if (definition.EnableMinorVersions.HasValue)
                assert.ShouldBeEqual(m => m.EnableMinorVersions, o => o.EnableMinorVersions);
            else
                assert.SkipProperty(m => m.EnableMinorVersions, "Skipping from validation. EnableMinorVersions IS NULL");

            if (definition.EnableModeration.HasValue)
                assert.ShouldBeEqual(m => m.EnableModeration, o => o.EnableModeration);
            else
                assert.SkipProperty(m => m.EnableModeration, "Skipping from validation. EnableModeration IS NULL");

            if (definition.EnableVersioning.HasValue)
                assert.ShouldBeEqual(m => m.EnableVersioning, o => o.EnableVersioning);
            else
                assert.SkipProperty(m => m.EnableVersioning, "Skipping from validation. EnableVersioning IS NULL");

            if (definition.ForceCheckout.HasValue)
                assert.ShouldBeEqual(m => m.ForceCheckout, o => o.ForceCheckout);
            else
                assert.SkipProperty(m => m.ForceCheckout, "Skipping from validation. ForceCheckout IS NULL");

            if (definition.NoCrawl.HasValue)
                assert.ShouldBeEqual(m => m.NoCrawl, o => o.NoCrawl);
            else
                assert.SkipProperty(m => m.NoCrawl, "Skipping from validation. NoCrawl IS NULL");


            if (definition.OnQuickLaunch.HasValue)
                assert.ShouldBeEqual(m => m.OnQuickLaunch, o => o.OnQuickLaunch);
            else
                assert.SkipProperty(m => m.OnQuickLaunch, "Skipping from validation. OnQuickLaunch IS NULL");


            // IRM
            if (definition.IrmEnabled.HasValue)
                assert.ShouldBeEqual(m => m.IrmEnabled, o => o.IrmEnabled);
            else
                assert.SkipProperty(m => m.IrmEnabled, "Skipping from validation. IrmEnabled IS NULL");

            if (definition.IrmExpire.HasValue)
                assert.ShouldBeEqual(m => m.IrmExpire, o => o.IrmExpire);
            else
                assert.SkipProperty(m => m.IrmExpire, "Skipping from validation. IrmExpire IS NULL");

            if (definition.IrmReject.HasValue)
                assert.ShouldBeEqual(m => m.IrmReject, o => o.IrmReject);
            else
                assert.SkipProperty(m => m.IrmReject, "Skipping from validation. IrmReject IS NULL");

            if (definition.TemplateType > 0)
            {
                assert.ShouldBeEqual(m => m.TemplateType, o => o.BaseTemplate);
            }
            else
            {
                assert.SkipProperty(m => m.TemplateType, "TemplateType == 0. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.TemplateName))
            {
                context.Load(web, tmpWeb => tmpWeb.ListTemplates);
                context.ExecuteQueryWithTrace();

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Fetching all list templates and matching target one.");
                var listTemplate = ResolveListTemplate(webModelHost, definition);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TemplateName);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid =
                            (spObject.TemplateFeatureId == listTemplate.FeatureId) &&
                            (spObject.BaseTemplate == listTemplate.ListTemplateTypeKind)
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.TemplateName, "TemplateName is null or empty. Skipping.");
            }

            if (definition.MajorVersionLimit.HasValue)
            {
                if (ReflectionUtils.HasProperty(spObject, "MajorVersionLimit"))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.MajorVersionLimit);
                        var value = (int)ReflectionUtils.GetPropertyValue(spObject, "MajorVersionLimit");

                        var isValid = value == definition.MajorVersionLimit.Value;

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
                    assert.SkipProperty(m => m.MajorVersionLimit, "Skipping from validation. MajorVersionLimit does not exist. CSOM runtime is below required.");
                }
            }
            else
                assert.SkipProperty(m => m.MajorVersionLimit, "Skipping from validation. MajorVersionLimit IS NULL");

            if (definition.MajorWithMinorVersionsLimit.HasValue)
            {
                if (ReflectionUtils.HasProperty(spObject, "MajorWithMinorVersionsLimit"))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.MajorWithMinorVersionsLimit);
                        var value = (int)ReflectionUtils.GetPropertyValue(spObject, "MajorWithMinorVersionsLimit");

                        var isValid = value == definition.MajorWithMinorVersionsLimit.Value;

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
                    assert.SkipProperty(m => m.MajorWithMinorVersionsLimit, "Skipping from validation. MajorWithMinorVersionsLimit does not exist. CSOM runtime is below required.");
                }
            }
            else
                assert.SkipProperty(m => m.MajorWithMinorVersionsLimit,
                    "Skipping from validation. MajorWithMinorVersionsLimit IS NULL");

            // template url
            if (string.IsNullOrEmpty(definition.DocumentTemplateUrl))
            {
                assert.SkipProperty(m => m.DocumentTemplateUrl, "Skipping DocumentTemplateUrl or library. Skipping.");
            }
            else
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.DocumentTemplateUrl);
                    var dstProp = d.DocumentTemplateUrl;

                    var srcUrl = srcProp.Value as string;
                    var dstUrl = dstProp;

                    if (!dstUrl.StartsWith("/"))
                        dstUrl = "/" + dstUrl;

                    bool isValid;

                    if (s.DocumentTemplateUrl.Contains("~sitecollection"))
                    {
                        var siteCollectionUrl = webModelHost.HostSite.ServerRelativeUrl == "/" ?
                                string.Empty : webModelHost.HostSite.ServerRelativeUrl;

                        isValid = srcUrl.Replace("~sitecollection", siteCollectionUrl) == dstUrl;
                    }
                    else if (s.DocumentTemplateUrl.Contains("~site"))
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
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }

            if (definition.IndexedRootFolderPropertyKeys.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.IndexedRootFolderPropertyKeys);

                    var isValid = false;

                    if (d.RootFolder.Properties.FieldValues.ContainsKey("vti_indexedpropertykeys"))
                    {
                        // check props, TODO

                        // check vti_indexedpropertykeys
                        var indexedPropertyKeys = d.RootFolder.Properties["vti_indexedpropertykeys"]
                                                   .ToString()
                                                   .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(es => Encoding.Unicode.GetString(System.Convert.FromBase64String(es)));

                        // Search if any indexPropertyKey from definition is not in WebModel
                        var differentKeys = s.IndexedRootFolderPropertyKeys.Select(o => o.Name)
                                                                 .Except(indexedPropertyKeys);

                        isValid = !differentKeys.Any();
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
                assert.SkipProperty(m => m.IndexedRootFolderPropertyKeys, "IndexedRootFolderPropertyKeys is NULL or empty. Skipping.");

            var supportsLocalization = ReflectionUtils.HasProperties(spObject, new[]
            {
                "TitleResource", "DescriptionResource"
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

                if (definition.DescriptionResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.DescriptionResource);
                        var isValid = true;

                        foreach (var userResource in s.DescriptionResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "DescriptionResource");

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
                    assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is NULL or empty. Skipping.");
                }
            }
            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      "CSOM runtime doesn't have Web.TitleResource and Web.DescriptionResource() methods support. Skipping validation.");

                assert.SkipProperty(m => m.TitleResource, "TitleResource is null or empty. Skipping.");
                assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is null or empty. Skipping.");
            }
        }
    }

    public static class Ex
    {
        public static string GetServerRelativeUrl(this ListDefinition listDef, Web web)
        {
#pragma warning disable 618
            return UrlUtility.CombineUrl(web.ServerRelativeUrl, listDef.GetListUrl());
#pragma warning restore 618
        }

        public static string GetServerRelativeUrl(this List list)
        {
            return list.RootFolder.ServerRelativeUrl;
        }
    }
}
