using System;
using System.Linq;

using Microsoft.SharePoint.Client;

using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Extensions;
using SPMeta2.Services;
using SPMeta2.Utils;
using System.Text;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWebDefinitionValidator : WebModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var hostClientContext = ExtractHostClientContext(modelHost);

            var parentWeb = ExtractWeb(modelHost);
            var definition = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var currentWebUrl = GetCurrentWebUrl(parentWeb.Context, parentWeb, definition);
            var spObject = GetExistingWeb(hostClientContext.Site, parentWeb, currentWebUrl);
            var context = spObject.Context;

            context.Load(spObject,
                            w => w.HasUniqueRoleAssignments,
                            w => w.Description,
                            w => w.Url,
                            w => w.Language,
                            w => w.WebTemplate,
                            w => w.Configuration,
                            w => w.Title,
                            w => w.Id,
                            w => w.AllProperties,
                            w => w.HasUniqueRoleAssignments,
                            w => w.AssociatedOwnerGroup,
                            w => w.AssociatedVisitorGroup,
                            w => w.AssociatedMemberGroup
                        );

            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                .ShouldBeEqual(m => m.LCID, o => o.GetLCID())
                .ShouldBeEqual(m => m.UseUniquePermission, o => o.HasUniqueRoleAssignments);

            if (!string.IsNullOrEmpty(definition.WebTemplate))
            {
                assert.ShouldBeEqual(m => m.WebTemplate, o => o.GetWebTemplate());
                assert.SkipProperty(m => m.CustomWebTemplate);
            }
            else
            {
                assert.SkipProperty(m => m.WebTemplate);
                assert.SkipProperty(m => m.CustomWebTemplate);
            }

            assert.ShouldBeEqualIfNotNullOrEmpty(m => m.Description, o => o.Description);

            assert.ShouldBeEqual((p, s, d) =>
            {
                if (!parentWeb.IsObjectPropertyInstantiated("Url"))
                {
                    parentWeb.Context.Load(parentWeb, o => o.Url);
                    parentWeb.Context.ExecuteQueryWithTrace();
                }

                var srcProp = s.GetExpressionValue(def => def.Url);
                var dstProp = d.GetExpressionValue(ct => ct.Url);

                var srcUrl = s.Url;
                var dstUrl = d.Url;

                srcUrl = UrlUtility.RemoveStartingSlash(srcUrl);

                var dstSubUrl = dstUrl.Replace(parentWeb.Url + "/", string.Empty);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = srcUrl == dstSubUrl
                };
            });

            var supportsAlternateCssAndSiteImageUrl = ReflectionUtils.HasProperties(spObject, new[]
            {
                "AlternateCssUrl",
                "SiteLogoUrl"
            });

            if (supportsAlternateCssAndSiteImageUrl)
            {
                // also check for MembersCanShare / RequestAccessEmail
                if (definition.MembersCanShare.HasValue)
                {
                    var membersCanShare = ConvertUtils.ToBool(ReflectionUtils.GetPropertyValue(spObject, "MembersCanShare")).Value;

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.MembersCanShare);
                        var isValid = true;

                        isValid = s.MembersCanShare.Value == membersCanShare;

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
                    assert.SkipProperty(m => m.MembersCanShare);
                }

                if (!string.IsNullOrEmpty(definition.RequestAccessEmail))
                {
                    var requestAccessEmail = ReflectionUtils.GetPropertyValue(spObject, "RequestAccessEmail");

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.RequestAccessEmail);
                        var isValid = true;

                        isValid = s.RequestAccessEmail.ToUpper() == requestAccessEmail.ToString().ToUpper();

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
                    assert.SkipProperty(m => m.RequestAccessEmail);
                }


                if (!string.IsNullOrEmpty(definition.AlternateCssUrl))
                {
                    var alternateCssUrl = ReflectionUtils.GetPropertyValue(spObject, "AlternateCssUrl");

                    assert.ShouldBeEqual((p, s, d) =>
                   {
                       var srcProp = s.GetExpressionValue(def => def.AlternateCssUrl);
                       var isValid = true;

                       isValid = s.AlternateCssUrl.ToUpper().EndsWith(alternateCssUrl.ToString().ToUpper());

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
                    assert.SkipProperty(m => m.AlternateCssUrl);
                }

                if (!string.IsNullOrEmpty(definition.SiteLogoUrl))
                {
                    var siteLogoUrl = ReflectionUtils.GetPropertyValue(spObject, "SiteLogoUrl");

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.SiteLogoUrl);

                        var isValid = s.SiteLogoUrl.ToUpper().EndsWith(siteLogoUrl.ToString().ToUpper());

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
                    assert.SkipProperty(m => m.SiteLogoUrl);
                }
            }
            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      "CSOM runtime doesn't have Web.AlternateCssUrl and Web.SiteLogoUrl methods support. Skipping validation.");

                assert.SkipProperty(m => m.AlternateCssUrl, "AlternateCssUrl is null or empty. Skipping.");
                assert.SkipProperty(m => m.SiteLogoUrl, "SiteLogoUrl is null or empty. Skipping.");

                assert.SkipProperty(m => m.MembersCanShare, "SiteLogoUrl is null or empty. Skipping.");
                assert.SkipProperty(m => m.RequestAccessEmail, "SiteLogoUrl is null or empty. Skipping.");
            }

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

            if (definition.IndexedPropertyKeys.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.IndexedPropertyKeys);

                    var isValid = false;

                    if (d.AllProperties.FieldValues.ContainsKey("vti_indexedpropertykeys"))
                    {
                        // check props, TODO

                        // check vti_indexedpropertykeys
                        var indexedPropertyKeys = d.AllProperties["vti_indexedpropertykeys"]
                                                   .ToString()
                                                   .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(es => Encoding.Unicode.GetString(System.Convert.FromBase64String(es)));

                        // Search if any indexPropertyKey from definition is not in WebModel
                        var differentKeys = s.IndexedPropertyKeys.Select(o => o.Name)
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
                assert.SkipProperty(m => m.IndexedPropertyKeys, "IndexedPropertyKeys is NULL or empty. Skipping.");

            // safe check - if not then we'll get the following exception
            // ---> System.InvalidOperationException: 
            // You cannot set this property since the web does not have unique permissions.
            if (definition.UseUniquePermission && spObject.HasUniqueRoleAssignments)
            {
                // AssociatedXXXGroupName
                if (!string.IsNullOrEmpty(definition.AssociatedMemberGroupName))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.AssociatedMemberGroupName);
                        var isValid = s.AssociatedVisitorGroupName == d.AssociatedVisitorGroup.Title;

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
                    assert.SkipProperty(m => m.AssociatedMemberGroupName, "AssociatedMemberGroupName is null");

                if (!string.IsNullOrEmpty(definition.AssociatedOwnerGroupName))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.AssociatedOwnerGroupName);
                        var isValid = s.AssociatedOwnerGroupName == d.AssociatedOwnerGroup.Title;

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
                    assert.SkipProperty(m => m.AssociatedOwnerGroupName, "AssociatedOwnerGroupName is null");


                if (!string.IsNullOrEmpty(definition.AssociatedVisitorGroupName))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.AssociatedVisitorGroupName);
                        var isValid = s.AssociatedVisitorGroupName == d.AssociatedVisitorGroup.Title;

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
                    assert.SkipProperty(m => m.AssociatedVisitorGroupName, "AssociatedVisitorGroup is null");
            }
            else
            {
                assert.SkipProperty(m => m.AssociatedVisitorGroupName, "AssociatedVisitorGroup is null");
                assert.SkipProperty(m => m.AssociatedOwnerGroupName, "AssociatedOwnerGroupName is null");
                assert.SkipProperty(m => m.AssociatedMemberGroupName, "AssociatedMemberGroupName is null");
            }
        }
    }
}
