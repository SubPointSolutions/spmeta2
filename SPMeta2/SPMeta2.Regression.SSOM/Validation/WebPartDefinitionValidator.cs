using System.Web.UI.WebControls;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using SPMeta2.SSOM.Extensions;
using System.Web.UI.WebControls.WebParts;
using SPMeta2.Exceptions;

using Microsoft.SharePoint;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Attributes.Regression;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WebPartDefinitionValidator : WebPartModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebPartDefinition>("model", value => value.RequireNotNull());

            var file = host.HostFile;
            var web = host.HostFile.Web;

            WebPartExtensions.WithExistingWebPart(file, definition, (spWebPartManager, spObject) =>
            {
                //[FALSE] - [Title]
                //                        [FALSE] - [Id]
                //                        [FALSE] - [ZoneId]
                //                        [FALSE] - [ZoneIndex]
                //                        [FALSE] - [WebpartFileName]
                //                        [FALSE] - [WebpartType]
                //                        [FALSE] - [WebpartXmlTemplate]

                var assert = ServiceFactory.AssertService
                                  .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);

                var currentType = spObject.GetType().AssemblyQualifiedName;
                var currentClassName = currentType.Split(',').First().Trim();

                var expectedTypeAttr = (definition.GetType().GetCustomAttributes(typeof(ExpectWebpartType))
                    .FirstOrDefault() as ExpectWebpartType);

                // NULL for the generic web part
                // should not be tested here
                if (expectedTypeAttr != null)
                {
                    var expectedType = expectedTypeAttr.WebPartType;

                    var expectedClassName = expectedType.Split(',').First().Trim();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var isValid = true;

                        isValid = currentClassName.ToUpper() == expectedClassName.ToUpper();

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = null,
                            Dst = null,
                            IsValid = isValid
                        };
                    });
                }
                // props

                if (definition.Properties.Count > 0)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var isValid = true;

                        foreach (var prop in definition.Properties)
                        {
                            // returns correct one depending on the V2/V3
                            var value = ReflectionUtils.GetPropertyValue(d, prop.Name);

                            // that True / true issue give a pain
                            // toLower for the time being
                            isValid = value.ToString().ToLower() == prop.Value.ToLower();

                            if (!isValid)
                                break;
                        }

                        var srcProp = s.GetExpressionValue(m => m.Properties);

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
                    assert.SkipProperty(m => m.Properties, "Properties are empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.ChromeState))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ChromeState);
                        var srcValue = (PartChromeState)Enum.Parse(typeof(PartChromeState), s.ChromeState);
                        var isValid = srcValue == d.ChromeState;

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
                    assert.SkipProperty(m => m.ChromeState, "ChromeType is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.ChromeType))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ChromeType);
                        var chromeType = WebPartChromeTypesConvertService.NormilizeValueToPartChromeTypes(s.ChromeType);

                        var srcValue = (PartChromeType)Enum.Parse(typeof(PartChromeType), chromeType);
                        var isValid = srcValue == d.ChromeType;

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
                    assert.SkipProperty(m => m.ChromeType, "ChromeType is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.Description))
                    assert.ShouldBeEqual(m => m.Description, o => o.Description);
                else
                    assert.SkipProperty(m => m.Description, "Description is null or empty. Skipping.");

                if (definition.Height.HasValue)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Height);
                        var isValid = d.Height == new Unit(s.Height.Value);

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
                    assert.SkipProperty(m => m.Height, "Height is null or empty. Skipping.");

                if (definition.Width.HasValue)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Width);
                        var isValid = d.Width == new Unit(s.Width.Value);

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
                    assert.SkipProperty(m => m.Width, "Width is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.ImportErrorMessage))
                    assert.ShouldBeEqual(m => m.ImportErrorMessage, o => o.ImportErrorMessage);
                else
                    assert.SkipProperty(m => m.ImportErrorMessage, "ImportErrorMessage is null or empty. Skipping.");

                if (!DisableTitleUrlValidation)
                {
                    if (!string.IsNullOrEmpty(definition.TitleUrl))
                    {
                        assert.ShouldBeEqual((p, s, d) =>
                        {
                            var srcProp = s.GetExpressionValue(m => m.TitleUrl);
                            var dstProp = d.GetExpressionValue(o => o.TitleUrl);

                            var srcUrl = srcProp.Value as string;
                            var dstUrl = dstProp.Value as string;

                            var isValid = false;

                            if (definition.TitleUrl.Contains("~sitecollection"))
                            {
                                var siteCollectionUrl = web.Site.ServerRelativeUrl == "/" ? string.Empty : web.Site.ServerRelativeUrl;

                                isValid = srcUrl.Replace("~sitecollection", siteCollectionUrl) == dstUrl;
                            }
                            else if (definition.TitleUrl.Contains("~site"))
                            {
                                var siteCollectionUrl = web.ServerRelativeUrl == "/" ? string.Empty : web.ServerRelativeUrl;

                                isValid = srcUrl.Replace("~site", siteCollectionUrl) == dstUrl;
                            }
                            else
                            {
                                isValid = srcUrl == dstUrl;
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
                    else
                        assert.SkipProperty(m => m.TitleUrl, "TitleUrl is null or empty. Skipping.");
                }

                if (!string.IsNullOrEmpty(definition.TitleIconImageUrl))
                {
                    assert.ShouldBeEndOf(m => m.TitleIconImageUrl, o => o.TitleIconImageUrl);
                }
                else
                    assert.SkipProperty(m => m.TitleIconImageUrl, "TitleIconImageUrl is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.ExportMode))
                {
                    assert.ShouldBeEqual(m => m.ExportMode, o => o.GetExportMode());
                }
                else
                    assert.SkipProperty(m => m.ExportMode, "ExportMode is null or empty. Skipping.");


                if (!string.IsNullOrEmpty(definition.Id))
                    assert.ShouldBeEqual(m => m.Id, o => o.ID);
                else
                    assert.SkipProperty(m => m.Id, "Id is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.Title))
                    assert.ShouldBeEqual(m => m.Title, o => o.Title);
                else
                    assert.SkipProperty(m => m.Title, "Title is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.ZoneId))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ZoneId);
                        //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = object.Equals(s.ZoneId, spWebPartManager.GetZoneID(d))
                        };
                    });
                }

                else
                    assert.SkipProperty(m => m.ZoneId, "ZoneId is null or empty. Skipping.");

                //if (definition.ZoneIndex > 0)
                //    assert.ShouldBeEqual(m => m.ZoneIndex, o => o.ZoneIndex);
                //else
                //    assert.SkipProperty(m => m.ZoneIndex, "ZoneIndex == 0. Skipping.");

                assert.SkipProperty(m => m.ZoneIndex, "Skipping.");

                if (!string.IsNullOrEmpty(definition.WebpartFileName))
                {
                    var site = host.HostFile.Web.Site;

                    var webPartManager = host.SPLimitedWebPartManager;

                    var webpartFileName = definition.WebpartFileName;
                    var rootWeb = site.RootWeb;

                    // load definition from WP catalog
                    var webpartCatalog = rootWeb.GetCatalog(SPListTemplateType.WebPartCatalog);
                    var webpartItem = webpartCatalog.Items.OfType<SPListItem>().FirstOrDefault(
                            i => string.Compare(i.Name, webpartFileName, true) == 0);

                    if (webpartItem == null)
                        throw new ArgumentException(string.Format("webpartItem. Can't find web part file with name: {0}", webpartFileName));

                    using (var streamReader = new MemoryStream(webpartItem.File.OpenBinary()))
                    {
                        using (var xmlReader = XmlReader.Create(streamReader))
                        {
                            var errMessage = string.Empty;
                            var webpartInstance = webPartManager.ImportWebPart(xmlReader, out errMessage);
                            var webPartInstanceType = webpartInstance.GetType();

                            assert.ShouldBeEqual((p, s, d) =>
                            {
                                var srcProp = s.GetExpressionValue(m => m.WebpartFileName);
                                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                                return new PropertyValidationResult
                                {
                                    Tag = p.Tag,
                                    Src = srcProp,
                                    Dst = null,
                                    IsValid = webPartInstanceType == d.GetType()
                                };
                            });
                        }
                    }
                }
                else
                    assert.SkipProperty(m => m.WebpartFileName, "ZoneIndex == 0. Skipping.");

                if (!string.IsNullOrEmpty(definition.WebpartType))
                {
                    var webPartInstance = WebPartExtensions.ResolveWebPartInstance(host.HostFile.Web.Site, host.SPLimitedWebPartManager, definition);
                    var webPartInstanceType = webPartInstance.GetType();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.WebpartType);
                        //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = webPartInstanceType == d.GetType()
                        };
                    });
                }
                else
                    assert.SkipProperty(m => m.WebpartType, "WebpartType is empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.WebpartXmlTemplate))
                {
                    assert.SkipProperty(m => m.WebpartXmlTemplate, "WebpartXmlTemplate is not empty, but no validation is required with SSOM. Skipping.");
                }
                else
                    assert.SkipProperty(m => m.WebpartXmlTemplate, "WebpartXmlTemplate is empty. Skipping.");

                if (definition.ParameterBindings.Count == 0)
                {
                    assert.SkipProperty(m => m.ParameterBindings, "ParameterBindings is null or empty. Skipping.");
                }
                else
                {


                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        // that's a fast hack
                        // hope we eoudn't have other web part with ParameterBindings :)
                        var webPart = d as DataFormWebPart;
                        var isValid = true;

                        var srcProp = s.GetExpressionValue(m => m.ParameterBindings);

                        // one more hack, fix up later
                        // just checking presense of the strings
                        foreach (var srcBinding in s.ParameterBindings)
                        {
                            var hasName = webPart.ParameterBindings.Contains(srcBinding.Name);
                            var hasValue = webPart.ParameterBindings.Contains(srcBinding.Location);

                            if (!hasName || !hasValue)
                            {
                                isValid = false;
                            }
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

                if (!string.IsNullOrEmpty(definition.AuthorizationFilter))
                    assert.ShouldBeEqual(m => m.AuthorizationFilter, o => o.AuthorizationFilter);
                else
                    assert.SkipProperty(m => m.AuthorizationFilter, "AuthorizationFilter is null or empty. Skipping.");

                if (definition.Hidden.HasValue)
                    assert.ShouldBeEqual(m => m.Hidden, o => o.Hidden);
                else
                    assert.SkipProperty(m => m.Hidden, "Hidden is null or empty. Skipping.");
            });
        }

        protected bool DisableTitleUrlValidation { get; set; }
    }

    internal static class WebpartEx
    {
        public static string GetZoneId(this WebPart webpart)
        {
            return webpart.Zone != null ? webpart.Zone.ID : string.Empty;
        }

        public static string GetExportMode(this WebPart webpart)
        {
            return webpart.ExportMode.ToString();
        }
    }
}
