using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSP2013WorkflowDefinitionValidator : SP2013WorkflowDefinitionHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SP2013WorkflowDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentWorkflowDefinition(webModelHost.HostWeb, definition);

            if (!spObject.IsPropertyAvailable("Xaml"))
            {
                var spObjectContext = spObject.Context;

                spObjectContext.Load(spObject, o => o.Xaml);
                spObjectContext.ExecuteQueryWithTrace();
            }

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .SkipProperty(m => m.Override, "Override is not supported yet.")
                                 .ShouldBeEqual(m => m.Xaml, o => o.Xaml)
                                 .ShouldBeEqual(m => m.DisplayName, o => o.DisplayName);


            if (!string.IsNullOrEmpty(definition.RestrictToScope))
                assert.ShouldBeEqual(m => m.RestrictToScope, o => o.RestrictToScope);
            else
                assert.SkipProperty(p => p.RestrictToScope);

            if (!string.IsNullOrEmpty(definition.RestrictToType))
                assert.ShouldBeEqual(m => m.RestrictToType, o => o.RestrictToType);
            else
                assert.SkipProperty(p => p.RestrictToType);

            if (definition.Properties.Count() > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Properties);
                    var dstProp = d.GetExpressionValue(ct => ct.Properties);

                    var isValid = true;

                    foreach (var prop in s.Properties)
                    {
                        var propName = prop.Name;
                        var propValue = prop.Value;

                        if (!d.Properties.ContainsKey(propName))
                        {
                            isValid = false;
                        }

                        if (d.Properties[propName] != propValue)
                        {
                            isValid = false;
                        }
                    }

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(p => p.Properties, ".Properties.Count() = 0. Skipping");
            }
        }
    }
}
