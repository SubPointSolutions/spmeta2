using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class QuickLaunchNavigationNodeDefinitionValidator : QuickLaunchNavigationNodeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // TMP
            if (modelHost is WebModelHost)
                CurrentWebModelHost = modelHost as WebModelHost;

            var definition = model.WithAssertAndCast<QuickLaunchNavigationNodeDefinition>("model", value => value.RequireNotNull());

            var spObject = LookupNodeForHost(modelHost, definition);
            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                 .ShouldNotBeNull(spObject)
                 //.ShouldBeEndOf(m => m.Url, o => o.Url)
                 .ShouldBeEqual(m => m.IsExternal, o => o.IsExternal)
                 .ShouldBeEqual(m => m.IsVisible, o => o.IsVisible)
                 .ShouldBeEqual(m => m.Title, o => o.Title);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Url);
                var dstProp = d.GetExpressionValue(ct => ct.Url);

                var srcUrl = s.Url;
                var dstUrl = d.Url;

                srcUrl = ResolveTokenizedUrl(CurrentWebModelHost, definition);

                var isValid = d.Url.ToUpper().EndsWith(srcUrl.ToUpper());

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
}
