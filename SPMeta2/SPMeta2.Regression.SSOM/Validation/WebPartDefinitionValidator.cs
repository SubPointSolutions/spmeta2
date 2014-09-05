using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WebPartDefinitionValidator : WebPartModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {


            //var assert = ServiceFactory.AssertService
            //                  .NewAssert(definition, spObject)
            //                        .ShouldNotBeNull(spObject)
            //                        .ShouldBeEqual(m => m.FileName, o => o.Name)
            //                        .ShouldBeEqual(m => m.Content, o => o.GetContent());

        }
    }
}
