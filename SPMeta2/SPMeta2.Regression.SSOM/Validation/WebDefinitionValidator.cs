using System;
using System.Linq.Expressions;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

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
                                 .ShouldBeEqual(m => m.Title, o => o.Title)
                                 .ShouldBeEqual(m => m.LCID, o => o.GetLCID())
                                 .ShouldBeEqual(m => m.WebTemplate, o => o.GetWebTemplate())
                                 .ShouldBeEqual(m => m.UseUniquePermission, o => o.HasUniqueRoleAssignments);

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
        }
    }

    internal static class WebExtensions
    {
        public static uint GetLCID(this SPWeb web)
        {
            return (uint)web.Locale.LCID;
        }

        public static string GetWebTemplate(this SPWeb web)
        {
            return string.Format("{0}#{1}", web.WebTemplate, web.Configuration);
        }
    }
}
