using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class InformationRightsManagementSettingsDefinitionValidator : InformationRightsManagementSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());

            var definition = model.WithAssertAndCast<InformationRightsManagementSettingsDefinition>("model", value => value.RequireNotNull());
            var spObject = GetCurrentInformationRightsManagementSettings(siteModelHost.HostList);

            var assert = ServiceFactory.AssertService
                  .NewAssert(definition, definition, spObject)
                  .ShouldNotBeNull(spObject)
                  .ShouldBeEqual(m => m.AllowPrint, o => o.AllowPrint)
                  .ShouldBeEqual(m => m.AllowScript, o => o.AllowScript)
                  .ShouldBeEqual(m => m.AllowWriteCopy, o => o.AllowWriteCopy)
                  .ShouldBeEqual(m => m.DisableDocumentBrowserView, o => o.DisableDocumentBrowserView)
                //.ShouldBeEqual(m => m.DocumentLibraryProtectionExpireDate, o => o.DocumentLibraryProtectionExpireDate)
                  .ShouldBeEqual(m => m.EnableDocumentAccessExpire, o => o.EnableDocumentAccessExpire)
                  .ShouldBeEqual(m => m.EnableDocumentBrowserPublishingView, o => o.EnableDocumentBrowserPublishingView)
                  .ShouldBeEqual(m => m.EnableGroupProtection, o => o.EnableGroupProtection)
                  .ShouldBeEqual(m => m.GroupName, o => o.GroupName)
                //.ShouldBeEqual(m => m.LicenseCacheExpireDays, o => o.LicenseCacheExpireDays)
                  .ShouldBeEqual(m => m.PolicyTitle, o => o.PolicyTitle)
                  .ShouldBeEqual(m => m.PolicyDescription, o => o.PolicyDescription)
                  .ShouldBeEqual(m => m.EnableLicenseCacheExpire, o => o.EnableLicenseCacheExpire)
                  .ShouldBeEqual(m => m.DocumentAccessExpireDays, o => o.DocumentAccessExpireDays);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.DocumentLibraryProtectionExpireDate);
                var dstProp = d.GetExpressionValue(ct => ct.DocumentLibraryProtectionExpireDate);

                var isValid = ((DateTime)srcProp.Value).ToShortDateString() == ((DateTime)dstProp.Value).ToShortDateString();

                return new PropertyValidationResult
                  {
                      Tag = p.Tag,
                      Src = srcProp,
                      Dst = dstProp,
                      IsValid = isValid
                  };
            });

            // by default, 30
            if (definition.EnableLicenseCacheExpire)
            {
                assert.ShouldBeEqual(m => m.LicenseCacheExpireDays, o => o.LicenseCacheExpireDays);
            }
            else
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.LicenseCacheExpireDays);
                    var dstProp = d.GetExpressionValue(ct => ct.LicenseCacheExpireDays);

                    var isValid = d.LicenseCacheExpireDays == 30;


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

        #endregion
    }
}
