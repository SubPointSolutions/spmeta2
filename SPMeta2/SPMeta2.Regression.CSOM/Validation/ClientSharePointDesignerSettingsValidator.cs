using System.Linq;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSharePointDesignerSettingsValidator : SharePointDesignerSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SharePointDesignerSettingsDefinition>("model", value => value.RequireNotNull());

            var spObject = webHost.HostSite.RootWeb;

            var context = spObject.Context;
            context.Load(spObject, s => s.AllProperties);
            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            // TODO

            //if (definition.DeleteAll)
            //{
            //    assert.ShouldBeEqual((p, s, d) =>
            //    {
            //        var srcProp = s.GetExpressionValue(def => def.DeleteAll);
            //        var isValid = true;

            //        isValid = spObject.RecycleBin.Count == 0;

            //        return new PropertyValidationResult
            //        {
            //            Tag = p.Tag,
            //            Src = srcProp,
            //            Dst = null,
            //            IsValid = isValid
            //        };
            //    });
            //}
            //else
            //{
            //    assert.SkipProperty(o => o.DeleteAll, "DeleteAll is false");
            //}
        }
    }

}
