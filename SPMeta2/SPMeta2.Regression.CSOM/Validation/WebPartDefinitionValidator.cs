using System.Net;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using SPMeta2.Attributes.Regression;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.Services;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class WebPartDefinitionValidator : WebPartModelHandler
    {
        protected XDocument CurrentWebPartXml { get; set; }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebPartDefinition>("model", value => value.RequireNotNull());

            var pageFile = listItemModelHost.HostFile;
            var context = pageFile.Context;

            context.Load(pageFile);
            context.ExecuteQueryWithTrace();

            var siteServerUrl = listItemModelHost.HostSite.ServerRelativeUrl;
            var webUrl = listItemModelHost.HostWeb.Url;

            var serverUrl = context.Url;

            if (siteServerUrl != "/")
                serverUrl = context.Url.Split(new string[] { siteServerUrl }, StringSplitOptions.RemoveEmptyEntries)[0];

            var absItemUrl = UrlUtility.CombineUrl(serverUrl, pageFile.ServerRelativeUrl);

            WithExistingWebPart(pageFile, definition, (spObject, spObjectDefintion) =>
            {
                var webpartExportUrl = UrlUtility.CombineUrl(new[]{
                        webUrl,
                        "_vti_bin/exportwp.aspx?pageurl=" + absItemUrl + "&" + "guidstring=" + spObjectDefintion.Id.ToString()
                    });

                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                var webClient = new WebClient();

                if (context.Credentials != null)
                {
                    webClient.Credentials = context.Credentials;
                    webClient.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                }
                else
                    webClient.UseDefaultCredentials = true;

                var webPartXmlString = webClient.DownloadString(webpartExportUrl);
                CurrentWebPartXml = WebpartXmlExtensions.LoadWebpartXmlDocument(webPartXmlString);


                assert.ShouldBeEqual(m => m.Title, o => o.Title);

                // checking the web part type, shoul be as expected
                // Add regression on 'expected' web part type #690

                var currentType = CurrentWebPartXml.GetWebPartAssemblyQualifiedName();
                var currentClassName = currentType.Split(',').First().Trim();

                var expectedTypeAttr = (definition.GetType().GetCustomAttributes(typeof(ExpectWebpartType))
                                                        .FirstOrDefault() as ExpectWebpartType);

                // NULL can be on generic web part
                // test should not care about that case, there other tests to enfore that attr usage
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
                            var value = CurrentWebPartXml.GetProperty(prop.Name);

                            // that True / true issue give a pain
                            // toLower for the time being
                            isValid = value.ToLower() == prop.Value.ToLower();

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


                if (!string.IsNullOrEmpty(definition.ExportMode))
                {
                    var value = CurrentWebPartXml.GetExportMode();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ExportMode);
                        var isValid = definition.ExportMode == value;

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
                    assert.SkipProperty(m => m.ExportMode, "ExportMode is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.ChromeState))
                {
                    var value = CurrentWebPartXml.GetChromeState();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ChromeState);
                        var isValid = definition.ChromeState == value;

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
                    assert.SkipProperty(m => m.ChromeState, "ChromeState is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.ChromeType))
                {
                    // returns correct one depending on the V2/V3
                    var value = CurrentWebPartXml.GetChromeType();

                    var chromeType = string.Empty;

                    if (CurrentWebPartXml.IsV3version())
                        chromeType = WebPartChromeTypesConvertService.NormilizeValueToPartChromeTypes(definition.ChromeType);
                    else if (CurrentWebPartXml.IsV2version())
                        chromeType = WebPartChromeTypesConvertService.NormilizeValueToFrameTypes(definition.ChromeType);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ChromeType);
                        var isValid = chromeType == value;

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
                {
                    var value = CurrentWebPartXml.GetProperty("Description");

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Description);
                        var isValid = (srcProp.Value as string) == value;

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
                    assert.SkipProperty(m => m.Description, "Description is null or empty. Skipping.");
                }

                if (definition.Height.HasValue)
                {
                    var value = ConvertUtils.ToInt(CurrentWebPartXml.GetProperty("Height").Replace("px", string.Empty));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Height);
                        var isValid = definition.Height == value;

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
                    var value = ConvertUtils.ToInt(CurrentWebPartXml.GetProperty("Width").Replace("px", string.Empty));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Width);
                        var isValid = definition.Width == value;

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
                    assert.SkipProperty(m => m.Width, "Width is NULL, skipping");
                }

                if (!string.IsNullOrEmpty(definition.ImportErrorMessage))
                {
                    var value = CurrentWebPartXml.GetImportErrorMessage();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ImportErrorMessage);
                        var isValid = definition.ImportErrorMessage == value;

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
                    assert.SkipProperty(m => m.ImportErrorMessage, "ImportErrorMessage is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.TitleIconImageUrl))
                {
                    var value = CurrentWebPartXml.GetTitleIconImageUrl();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.TitleIconImageUrl);
                        var isValid = definition.TitleIconImageUrl == value;

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
                    assert.SkipProperty(m => m.TitleIconImageUrl, "TitleIconImageUrl is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.TitleUrl))
                {
                    var value = CurrentWebPartXml.GetTitleUrl();
                    var defValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Context = listItemModelHost,
                        Value = value
                    }).Value;

                    var isValid = defValue.ToUpper() == value.ToUpper();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.TitleUrl);

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
                    assert.SkipProperty(m => m.TitleUrl, "TitleUrl is null or empty. Skipping.");


                assert.SkipProperty(m => m.WebpartFileName, "WebpartFileName is null or empty. Skipping.");
                assert.SkipProperty(m => m.WebpartType, "WebpartType is null or empty. Skipping.");
                assert.SkipProperty(m => m.WebpartXmlTemplate, "WebpartXmlTemplate is null or empty. Skipping.");

                assert.SkipProperty(m => m.ZoneId, "ZoneId is null or empty. Skipping.");
                assert.SkipProperty(m => m.ZoneIndex, "ZoneIndex is null or empty. Skipping.");

                assert.SkipProperty(m => m.Id, "Id is null or empty. Skipping.");

                if (definition.ParameterBindings.Count == 0)
                {
                    assert.SkipProperty(m => m.ParameterBindings, "ParameterBindings is null or empty. Skipping.");
                }
                else
                {
                    // TODO
                }
            });
        }
    }
}
