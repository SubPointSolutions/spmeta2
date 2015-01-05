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
    public class RootWebDefinitionValidator : RootWebModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentObject(siteModelHost, definition);

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var isValid = d.IsRootWeb;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = null,
                    Dst = null,
                    IsValid = isValid,
                    Message = "Checking if IsRootWeb == TRUE"
                };
            });
        }
    }

}
