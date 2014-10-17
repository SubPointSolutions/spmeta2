using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FieldDefinitionValidator : FieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            var site = typedModelHost.HostSite;
            var spObject = GetField(modelHost, definition);

            ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldBeEqual(m => m.Title, o => o.Title)
                                 .ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                                 .ShouldBeEqual(m => m.Group, o => o.Group)
                                 .ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
                                 .ShouldBeEqual(m => m.Id, o => o.Id)
                                 .ShouldBeEqual(m => m.Description, o => o.Description)
                                 .ShouldBeEqual(m => m.Required, o => o.Required);
        }
    }
}
