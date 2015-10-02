﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers.ContentTypes;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.ContentTypes
{
    public class UniqueContentTypeFieldsOrderDefinitionValidator : UniqueContentTypeFieldsOrderModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<UniqueContentTypeFieldsOrderDefinition>("model", value => value.RequireNotNull());
            var spObject = ExtractContentTypeFromHost(modelHost);

            spObject.Context.Load(spObject, c => c.Fields);
            spObject.Context.Load(spObject, c => c.FieldLinks);
            spObject.Context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                             .NewAssert(definition, spObject)
                                   .ShouldNotBeNull(spObject);

            var srcOrder = definition.Fields
               .Select(f => f.Id.HasValue
                   ? spObject.Fields.ToList().FirstOrDefault(spField => spField.Id == f.Id.Value).Id
                   : spObject.Fields.ToList().FirstOrDefault(spField => spField.InternalName == f.InternalName).Id)
                  .ToList();
            var dstOrder = spObject.FieldLinks
                .ToList()
                .Select(l => l.Id)
                .ToList();

            assert
               .ShouldBeEqual((p, s, d) =>
               {
                   var srcProp = s.GetExpressionValue(def => def.Fields);
                   var dstProp = d.GetExpressionValue(ct => ct.FieldLinks);

                   var hasCorrrectOrder = true;
                   var dstOrderList = new List<int>();

                   for (int srcOrderIndex = 0; srcOrderIndex < srcOrder.Count(); srcOrderIndex++)
                   {
                       var srcFieldId = srcOrder[srcOrderIndex];
                       var dstOrderIndex = dstOrder.IndexOf(dstOrder.FirstOrDefault(i => i == srcFieldId));

                       dstOrderList.Add(dstOrderIndex);

                       if (dstOrderList.Count > 1)
                       {
                           hasCorrrectOrder = dstOrderList[dstOrderList.Count() - 1] >
                                              dstOrderList[dstOrderList.Count() - 2];
                       }
                   }

                   return new PropertyValidationResult
                   {
                       Tag = p.Tag,
                       Src = srcProp,
                       Dst = dstProp,
                       IsValid = hasCorrrectOrder
                   };
               });
        }
    }
}
