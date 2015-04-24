using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

using Microsoft.VisualStudio.TestTools.UnitTesting;


using SPMeta2.CSOM.Extensions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientContentTypeDefinitionValidator : ContentTypeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            //var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var site = ExtractSite(modelHost);
            var web = ExtractWeb(modelHost);

            var context = web.Context;
            var rootWeb = web;

            var contentTypes = rootWeb.ContentTypes;

            context.Load(rootWeb);
            context.Load(contentTypes);

            context.ExecuteQuery();

            var contentTypeId = definition.GetContentTypeId();
            var spObject = contentTypes.FindByName(definition.Name);

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Name, o => o.Name)
                .ShouldBeEqual(m => m.Group, o => o.Group)
                .ShouldBeEqual(m => m.Hidden, o => o.Hidden);
                  //.ShouldBeEqual(m => m.Description, o => o.Description);


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
                        var siteCollectionUrl = site.ServerRelativeUrl == "/" ? string.Empty : site.ServerRelativeUrl;

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
        }
    }

    internal static class ContentTypeDefinitionValidatorUtils
    {
        public static string GetId(this ContentType c)
        {
            return c.Id.ToString();
        }
    }
}
