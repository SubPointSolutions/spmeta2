using System;
using SPMeta2.Containers.Assertion;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Regression.Definitions.Extended;
using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Validation.Extended
{
    public class XsltListViewWebPartGridModePresenceValidator : WebpartPresenceOnPageDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(XsltListViewWebPartGridModePresenceDefinition); }
        }

        protected override void ValidateHtmlPage(SPFile file, string pageUrl, string pageContent, DefinitionBase definitionBase)
        {
            var definition = definitionBase as XsltListViewWebPartGridModePresenceDefinition;

            var assert = ServiceFactory.AssertService
               .NewAssert(definition, file)
                // dont' need to check that, not the pupose of the test
                //.ShouldBeEqual(m => m.PageFileName, o => o.GetName())
               .ShouldNotBeNull(file);

            //if (definition.WebPartDefinitions.Any())
            //{
            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.WebPartDefinitions);

                var isValid = true;

                TraceUtils.WithScope(trace =>
                {
                    trace.WriteLine(string.Format("Checking InitGridFromView presence:[{0}]", pageUrl));

                    isValid = pageContent.Contains("InitGridFromView");
                });

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    //Dst = dstProp,
                    IsValid = isValid
                };
            });
            //}
            //else
            //{
            //    assert.SkipProperty(m => m.WebPartDefinitions, "WebPartDefinitions.Count = 0. Skipping");
            //}
        }
    }
}
