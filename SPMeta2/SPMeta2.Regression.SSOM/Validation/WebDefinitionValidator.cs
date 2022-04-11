using System.Linq;

using Microsoft.SharePoint;

using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Extensions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WebDefinitionValidator : WebModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());
            SPWeb parentWeb = null;

            if (modelHost is SiteModelHost)
                parentWeb = (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                parentWeb = (modelHost as WebModelHost).HostWeb;

            var spObject = GetWeb(parentWeb, definition);

            var assert = ServiceFactory.AssertService
                       .NewAssert(definition, spObject)
                       .ShouldBeEqual(m => m.LCID, o => o.GetLCID());
            //.ShouldBeEqual(m => m.WebTemplate, o => o.GetWebTemplate())

            // temporarily switch culture to allow setting of the properties Title and Description for multi-language scenarios
            CultureUtils.WithCulture(spObject.UICulture, () =>
            {
                assert.ShouldBeEqual(m => m.Title, o => o.Title)
                    .ShouldBeEqual(m => m.UseUniquePermission, o => o.HasUniqueRoleAssignments);
            });

            if (!string.IsNullOrEmpty(definition.WebTemplate))
            {
                assert.ShouldBeEqual(m => m.WebTemplate, o => o.GetWebTemplate());
                assert.SkipProperty(m => m.CustomWebTemplate);
            }
            else
            {
                // no sense to chek custom web template
                assert.SkipProperty(m => m.WebTemplate);
                assert.SkipProperty(m => m.CustomWebTemplate);
            }

            if (!string.IsNullOrEmpty(definition.AlternateCssUrl))
                assert.ShouldBeEndOf(m => m.AlternateCssUrl, o => o.AlternateCssUrl);
            else
                assert.SkipProperty(m => m.AlternateCssUrl);

            if (!string.IsNullOrEmpty(definition.SiteLogoUrl))
                assert.ShouldBeEndOf(m => m.SiteLogoUrl, o => o.SiteLogoUrl);
            else
                assert.SkipProperty(m => m.SiteLogoUrl);

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Url);
                var dstProp = d.GetExpressionValue(ct => ct.Url);

                var srcUrl = s.Url;
                var dstUrl = d.Url;

                srcUrl = UrlUtility.RemoveStartingSlash(srcUrl);

                var dstSubUrl = dstUrl.Replace(parentWeb.Url + "/", string.Empty);
                var isValid = srcUrl.ToUpper() == dstSubUrl.ToUpper();

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

            // localization
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

            if (definition.IndexedPropertyKeys.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.IndexedPropertyKeys);

                    // Search if any indexPropertyKey from definition is not in WebModel
                    var differentKeys = s.IndexedPropertyKeys.Select(o => o.Name)
                                                             .Except(d.IndexedPropertyKeys);

                    var isValid = !differentKeys.Any();

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


            // O365 props
            assert.SkipProperty(m => m.MembersCanShare, "Skipping O365 prop");
            assert.SkipProperty(m => m.RequestAccessEmail, "Skipping O365 prop");

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
                        var isValid = s.AssociatedVisitorGroupName == d.AssociatedVisitorGroup.Name;

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
                        var isValid = s.AssociatedOwnerGroupName == d.AssociatedOwnerGroup.Name;

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
                        var isValid = s.AssociatedVisitorGroupName == d.AssociatedVisitorGroup.Name;

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
