using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using Microsoft.SharePoint;

using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FeatureDefinitionValidator : FeatureModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FeatureDefinition>("model", value => value.RequireNotNull());

            SPFeatureCollection features = null;
            SPFeature spObject = null;

            var assert = ServiceFactory.AssertService
                   .NewAssert(definition, spObject);

            switch (definition.Scope)
            {
                case FeatureDefinitionScope.Farm:
                    assert.SkipProperty(m => m.Scope, "Correct farm scope");

                    var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
                    var farm = farmModelHost.HostFarm;

                    var adminService = SPWebService.AdministrationService;

                    features = adminService.Features;

                    break;

                case FeatureDefinitionScope.WebApplication:

                    assert.SkipProperty(m => m.Scope, "Correct web app scope");

                    var webApplicationModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
                    var webApplication = webApplicationModelHost.HostWebApplication;

                    features = webApplication.Features;

                    break;

                case FeatureDefinitionScope.Site:

                    assert.SkipProperty(m => m.Scope, "Correct site scope");

                    var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
                    features = siteModelHost.HostSite.Features;
                    break;

                case FeatureDefinitionScope.Web:

                    assert.SkipProperty(m => m.Scope, "Correct web scope");

                    var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
                    features = webModelHost.HostWeb.Features;
                    break;
            }

            spObject = GetFeature(features, definition);
            assert.Dst = spObject;

            assert
                .ShouldBeEqual(m => m.Id, o => o.GetFeatureId());

            if (definition.ForceActivate)
            {
                assert
                    .SkipProperty(m => m.ForceActivate, "ForceActivate = true. Expect not null feature instance.")
                    .ShouldNotBeNull(spObject);
            }
            else
            {
                assert
                  .SkipProperty(m => m.ForceActivate, "ForceActivate = false. Skipping.");
            }


            if (definition.Enable)
            {
                assert
                    .SkipProperty(m => m.Enable, "Enable = true. Expect not null feature instance.")
                    .ShouldNotBeNull(spObject);
            }
            else
            {
                assert
                  .SkipProperty(m => m.Enable, "Enable = false. Expect null feature instance.")
                  .ShouldBeNull(spObject);
            }
        }
    }

    internal static class SPFeatureExtensions
    {
        public static Guid GetFeatureId(this SPFeature feature)
        {
            return feature.Definition.Id;
        }
    }
}
