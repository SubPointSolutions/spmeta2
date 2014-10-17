using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Fields
{
    public class TaxonomyFieldDefinitionValidator : TaxonomyFieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());

            var site = typedModelHost.HostSite;
            var spObject = GetField(modelHost, definition) as TaxonomyField;

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
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
