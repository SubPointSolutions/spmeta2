using System.Collections.Generic;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Standard.Validation.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.DisplayTemplates
{
    public class ItemDisplayTemplateDefinitionValidator : TemplateDefinitionBaseValidator
    {
        public override string FileExtension
        {
            get { return "html"; }
            set { }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);
                
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ItemDisplayTemplateDefinition>("model", value => value.RequireNotNull());
            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);

            var assert = ServiceFactory.AssertService
                                  .NewAssert(definition, spObject);

            assert.ShouldBeEqual(m => m.ManagedPropertyMappings, o => o.GetManagedPropertyMapping());
        }

        public override System.Type TargetType
        {
            get { return typeof(ItemDisplayTemplateDefinition); }
        }
    }
}
