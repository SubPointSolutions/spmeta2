using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;

using SPMeta2.Utils;

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

            if (!spObject.IsObjectPropertyInstantiated("HasUniqueRoleAssignments"))
                context.Load(spObject, o => o.HasUniqueRoleAssignments);

            if (!spObject.IsObjectPropertyInstantiated("Description"))
                context.Load(spObject, o => o.Description);

            if (!spObject.IsObjectPropertyInstantiated("Url"))
                context.Load(spObject, o => o.Url);

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldBeEqual(m => m.Title, o => o.Title)
                                 .ShouldBeEqual(m => m.LCID, o => o.GetLCID())
                                 .ShouldBeEqual(m => m.WebTemplate, o => o.GetWebTemplate())
                                 .ShouldBeEqual(m => m.UseUniquePermission, o => o.HasUniqueRoleAssignments)
                                 .ShouldBeEqual(m => m.Description, o => o.Description);

            assert.ShouldBeEqual((p, s, d) =>
            {
                if (!parentWeb.IsObjectPropertyInstantiated("Url"))
                {
                    parentWeb.Context.Load(parentWeb, o => o.Url);
                    parentWeb.Context.ExecuteQuery();
                }

                var srcProp = s.GetExpressionValue(def => def.Url);
                var dstProp = d.GetExpressionValue(ct => ct.Url);

                var srcUrl = s.Url;
                var dstUrl = d.Url;

                var dstSubUrl = dstUrl.Replace(parentWeb.Url + "/", string.Empty);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = srcUrl == dstSubUrl
                };
            });
        }
    }

    internal static class WebExtensions
    {
        public static uint GetLCID(this Web web)
        {
            return (uint)web.Language;
        }

        public static string GetWebTemplate(this Web web)
        {
            return string.Format("{0}#{1}", web.WebTemplate, web.Configuration);
        }
    }
}
