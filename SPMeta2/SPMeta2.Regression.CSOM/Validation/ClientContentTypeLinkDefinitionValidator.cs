using System.Diagnostics;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.Regression.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientContentTypeLinkDefinitionValidator : ContentTypeLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var context = list.Context;

            context.Load(list, l => l.ContentTypesEnabled);
            context.Load(list, l => l.ContentTypes);

            context.ExecuteQuery();

            var spObject = FindListContentType(list, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                            .ShouldNotBeNull(spObject);


            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert
                    .ShouldBeEqual(m => m.ContentTypeName, o => o.Name);
            }
            else
            {
                assert
                    .SkipProperty(m => m.ContentTypeName, "ContentTypeName is empty. Skipping");

            }

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeId);
                    var dstProp = d.GetExpressionValue(ct => ct.Id);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = dstProp.Value.ToString().ToUpper().StartsWith(srcProp.Value.ToString().ToUpper())
                    };
                });
            }
            else
            {
                assert
                    .SkipProperty(m => m.ContentTypeId, "ContentTypeId is empty. Skipping");

            }
        }
    }
}
