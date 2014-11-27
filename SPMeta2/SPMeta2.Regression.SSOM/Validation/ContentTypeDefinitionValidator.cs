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


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentTypeDefinitionValidator : ContentTypeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var rootWeb = site.RootWeb;

            var contentTypes = rootWeb.AvailableContentTypes;
            var spObject = contentTypes[definition.Name];

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                  .ShouldBeEqual(m => m.Name, o => o.Name)
                  .ShouldBeEqual(m => m.Group, o => o.Group)
                  .ShouldBeEqual(m => m.Description, o => o.Description);

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
        }
    }

    internal static class ContentTypeDefinitionValidatorUtils
    {
        public static string GetId(this SPContentType c)
        {
            return c.Id.ToString();
        }
    }
}
