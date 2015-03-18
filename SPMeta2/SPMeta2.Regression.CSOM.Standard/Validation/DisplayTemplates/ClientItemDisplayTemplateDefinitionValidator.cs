using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.DisplayTemplates
{
    public class ClientItemDisplayTemplateDefinitionValidator : ItemDisplayTemplateModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<ItemDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            var spObject = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);

            var context = spObject.Context;

            context.Load(spObject);

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);

        }

        #endregion
    }
}
