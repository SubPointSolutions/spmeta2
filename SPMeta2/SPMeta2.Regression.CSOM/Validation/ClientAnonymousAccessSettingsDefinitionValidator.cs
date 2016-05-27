using System;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;


namespace SPMeta2.Regression.CSOM.Validation
{
    public class AnonymousAccessSettingsDefinitionValidator : AnonymousAccessSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<AnonymousAccessSettingsDefinition>("model",
                value => value.RequireNotNull());

            var spObject = webModelHost.HostWeb;

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            assert.SkipProperty(m => m.AnonymousPermMask64, "Skipping due to CSOM API limitation.");
            assert.SkipProperty(m => m.AnonymousState, "Skipping due to CSOM API limitation.");

            //SSOm port

            //if (definition.AnonymousState == BuiltInWebAnonymousState.Disabled)
            //{
            //    assert.SkipProperty(m => m.AnonymousPermMask64, "AnonymousState is disables");
            //}
            //else
            //{
            //    assert.ShouldBeEqual((p, s, d) =>
            //    {
            //        var srcProp = s.GetExpressionValue(def => def.AnonymousPermMask64);
            //        var dstProp = d.GetExpressionValue(ct => ct.AnonymousPermMask64);

            //        var hasCorrectRights = true;

            //        foreach (var srcRight in s.AnonymousPermMask64)
            //        {
            //            var srcPermission = (SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), srcRight);

            //            var tmpRight = d.AnonymousPermMask64.HasFlag(srcPermission);

            //            if (tmpRight == false)
            //                hasCorrectRights = false;
            //        }

            //        return new PropertyValidationResult
            //        {
            //            Tag = p.Tag,
            //            Src = srcProp,
            //            Dst = dstProp,
            //            IsValid = hasCorrectRights
            //        };
            //    });
            //}

            //assert.ShouldBeEqual(m => m.AnonymousState, o => o.AnonymousState.ToString());
        }
    }
}