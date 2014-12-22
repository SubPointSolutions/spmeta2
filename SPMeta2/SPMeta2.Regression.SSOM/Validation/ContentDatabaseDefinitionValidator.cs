using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentDatabaseDefinitionValidator : ContentDatabaseModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentDatabaseDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentContentDatabase(webAppModelHost.HostWebApplication, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                       .ShouldBeEqual(m => m.DbName, o => o.Name)
                                       .ShouldBeEqual(m => m.MaximumSiteCollectionNumber, o => o.MaximumSiteCount)
                                       .ShouldBeEqual(m => m.WarningSiteCollectionNumber, o => o.WarningSiteCount);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.ServerName);
                var dstProp = d.GetExpressionValue(m => m.Server);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = s.ServerName.ToUpper() == d.Server.ToUpper()
                };
            });

        }
    }
}
