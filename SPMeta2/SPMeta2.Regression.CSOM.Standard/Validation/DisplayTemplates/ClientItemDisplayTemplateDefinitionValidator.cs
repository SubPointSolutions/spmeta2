using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Standard.Validation.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.DisplayTemplates
{
    public class ClientItemDisplayTemplateDefinitionValidator : ItemControlTemplateDefinitionBaseValidator
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<ItemDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            var file = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = file.ListItemAllFields;

            var context = spObject.Context;

            context.Load(spObject);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);

            if (!string.IsNullOrEmpty(definition.ManagedPropertyMappings))
                assert.ShouldBeEqual(m => m.ManagedPropertyMappings, o => o.GetManagedPropertyMapping());
            else
                assert.SkipProperty(m => m.ManagedPropertyMappings, "ManagedPropertyMappings is null or empty Skipping.");

           
        }

        #endregion

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {

        }

        public override System.Type TargetType
        {
            get { return typeof(ItemDisplayTemplateDefinition); }
        }
    }
}
