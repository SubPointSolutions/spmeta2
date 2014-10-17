using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using Microsoft.SharePoint;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class UserCustomActionDefinitionValidator : UserCustomActionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<UserCustomActionDefinition>("model", value => value.RequireNotNull());
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var spObject = GetCurrentCustomUserAction(site, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, definition, spObject)
                                            .ShouldNotBeNull(spObject)
                                            .ShouldBeEqual(m => m.Name, o => o.Name)
                                            .ShouldBeEqual(m => m.Title, o => o.Title)
                                            .ShouldBeEqual(m => m.Description, o => o.Description)
                                            .ShouldBeEqual(m => m.Group, o => o.Group)
                                            .ShouldBeEqual(m => m.Location, o => o.Location)
                                            .ShouldBeEqual(m => m.ScriptSrc, o => o.ScriptSrc)
                                            .ShouldBeEqual(m => m.ScriptBlock, o => o.ScriptBlock)
                                            .ShouldBeEqual(m => m.Sequence, o => o.Sequence)
                                            .ShouldBeEqual(m => m.Url, o => o.Url)
                                            .ShouldBeEqual(m => m.RegistrationId, o => o.RegistrationId)
                                            .ShouldBeEqual(m => m.RegistrationType, o => o.GetRegistrationType());


            assert
                .ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Rights);
                    var dstProp = d.GetExpressionValue(ct => ct.Rights);

                    var hasCorrectRights = true;

                    foreach (var srcRight in s.Rights)
                    {
                        var srcPermission = (SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), srcRight);

                        var tmpRight = d.Rights.HasFlag(srcPermission);

                        if (tmpRight == false)
                            hasCorrectRights = false;
                    }

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = hasCorrectRights
                    };
                });

        }
    }

    internal static class SPUserActionHelpers
    {
        public static string GetRegistrationType(this SPUserCustomAction action)
        {
            return action.RegistrationType.ToString();
        }
    }
}
