using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class OfficialFileHostDefinitionValidator : OfficialFileHostModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<OfficialFileHostDefinition>("model", value => value.RequireNotNull());

            var spObject = FindExistingObject(typedModelHost.HostWebApplication, typedDefinition);

            var assert = ServiceFactory.AssertService
                               .NewAssert(typedDefinition, spObject)
                               .ShouldNotBeNull(spObject)
                               .ShouldBeEqual(m => m.OfficialFileName, o => o.OfficialFileName)
                               .ShouldBeEqual(m => m.ShowOnSendToMenu, o => o.ShowOnSendToMenu);

            if (!string.IsNullOrEmpty(typedDefinition.Explanation))
                assert.ShouldBeEqual(m => m.Explanation, o => o.Explanation);
            else
                assert.SkipProperty(m => m.Explanation);

            if (!string.IsNullOrEmpty(typedDefinition.Explanation))
                assert.ShouldBeEqual(m => m.Explanation, o => o.Explanation);
            else
                assert.SkipProperty(m => m.Explanation);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.OfficialFileUrl);
                var dstProp = d.GetExpressionValue(m => m.OfficialFileUrl);

                var isValid = srcProp.ToString() == dstProp.ToString();

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Action);
                var dstProp = d.GetExpressionValue(m => m.Action);

                var isValid = srcProp.ToString() == dstProp.ToString();

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });


            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.CreateUniqueId);
                var dstProp = d.GetExpressionValue(m => m.UniqueId);

                var isValid = s.CreateUniqueId
                                ? d.UniqueId != default(Guid)
                                : d.UniqueId == default(Guid);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

        }

        #endregion
    }
}
