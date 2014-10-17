using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.Regression.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentTypeFieldLinkDefinitionValidator : ContentTypeFieldLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var spModel = modelHost.WithAssertAndCast<SPContentType>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var spObject = spModel.FieldLinks[definition.FieldId];

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldBeEqual(m => m.FieldId, o => o.Id);
        }
    }
}
