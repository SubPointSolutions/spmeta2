using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientQuickLaunchNavigationNodeDefinitionValidator : QuickLaunchNavigationNodeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (modelHost is WebModelHost)
                CurrentClientContext = (modelHost as WebModelHost).HostClientContext;

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

                srcUrl = ResolveTokenizedUrl(CurrentClientContext, definition);

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
