using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers.ContentTypes;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;
using SPMeta2.CSOM.Extensions;
namespace SPMeta2.Regression.CSOM.Validation.ContentTypes
{
    public class RemoveContentTypeLinksDefinitionValidator : RemoveContentTypeLinksModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<RemoveContentTypeLinksDefinition>("model", value => value.RequireNotNull());
            var list = ExtractListFromHost(modelHost);
            var spObject = ExtractFolderFromHost(modelHost);

            var context = spObject.Context;

            context.Load(list, l => l.ContentTypes.Include(
               ct => ct.Id,
               ct => ct.StringId,
               ct => ct.Name,
               ct => ct.ReadOnly,

               ct => ct.Parent.Id
               ));

            context.Load(spObject, f => f.UniqueContentTypeOrder);
            context.ExecuteQueryWithTrace();

            var listContentTypes = list.ContentTypes;

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldNotBeNull(list);

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
                        var spContentTypeId = srcContentTypeDef.ContentTypeId.ToUpper();
                        var listContentType = listContentTypes.FirstOrDefault(c => c.StringId.StartsWith(spContentTypeId));

                        exists = listContentType != null;

                    }
                    else if (!string.IsNullOrEmpty(srcContentTypeDef.ContentTypeName))
                    {
                        var listContentType = listContentTypes.FirstOrDefault(c => c.Name.ToUpper() == srcContentTypeDef.ContentTypeName.ToUpper());
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
