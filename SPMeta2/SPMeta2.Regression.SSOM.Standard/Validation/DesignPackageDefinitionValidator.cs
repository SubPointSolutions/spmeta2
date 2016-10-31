using System;
using System.Linq;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Extensions;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class DesignPackageDefinitionValidator : DesignPackageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DesignPackageDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var rootWeb = site.RootWeb;

            var spObject = FindExistingSolutionById(siteModelHost, definition.SolutionId);

            var assert = ServiceFactory.AssertService.NewAssert(definition, definition, spObject)
                                .ShouldNotBeNull(spObject)
                                .ShouldBeEqual(m => m.SolutionId, o => o.SolutionId)
                                .SkipProperty(m => m.FileName, "Solution should be deployed fine.")
                                .SkipProperty(m => m.Content, "Solution should be deployed fine.");

            if (definition.Apply)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    // once applied, design package sets AppliedDesignGuid property in the root web
                    // checking is all good
                    var installedPackageId = ConvertUtils.ToGuid(rootWeb.AllProperties["AppliedDesignGuid"]);

                    var srcProp = s.GetExpressionValue(m => m.Apply);
                    var isValid = spObject != null
                                  && installedPackageId == definition.SolutionId;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.Apply, "Apply is false");
            }

            if (definition.Install)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.Install);
                    var isValid = spObject != null;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.Install, "Install is false");
            }

            assert.SkipProperty(m => m.MajorVersion, string.Empty);
            assert.SkipProperty(m => m.MinorVersion, string.Empty);
        }

        #endregion
    }
}
