using System;
using System.Linq;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHandlers.ContentTypes;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation.ContentTypes
{
    public class HideContentTypeLinksDefinitionValidator : HideContentTypeLinksModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<HideContentTypeLinksDefinition>("model", value => value.RequireNotNull());
            
            var spObject = ExtractFolderFromHost(modelHost);
            var contentTypeOrder = spObject.UniqueContentTypeOrder;

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldNotBeNull(contentTypeOrder);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.ContentTypes);
                var dstProp = d.GetExpressionValue(ct => ct.UniqueContentTypeOrder);

                var isValid = true;

                foreach (var srcContentTypeDef in s.ContentTypes)
                {
                    var exists = false;

                    if (!string.IsNullOrEmpty(srcContentTypeDef.ContentTypeId))
                    {
                        var spContentTypeId = new SPContentTypeId(srcContentTypeDef.ContentTypeId);
                        var listContentType = contentTypeOrder.FirstOrDefault(c => c.Parent.Id == spContentTypeId);

                        exists = listContentType != null;
                    }
                    else if (!string.IsNullOrEmpty(srcContentTypeDef.ContentTypeName))
                    {
                        var listContentType = contentTypeOrder.FirstOrDefault(c => c.Name.ToUpper() == srcContentTypeDef.ContentTypeName.ToUpper());
                        exists = listContentType != null;
                    }

                    if (exists)
                    {
                        isValid = false;
                        break;
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
    }
}
