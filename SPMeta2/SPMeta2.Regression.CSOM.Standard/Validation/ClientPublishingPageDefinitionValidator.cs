using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ClientPublishingPageDefinitionValidator : PublishingPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;
            var definition = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            var spObject = FindPublishingPage(folderModelHost.CurrentList, folder, definition);

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spObject, o => o.DisplayName);
            context.Load(spObject, o => o.ContentType);

            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject)
                                           .ShouldBeEqual(m => m.FileName, o => o.GetFileName())
                                           //.ShouldBeEqual(m => m.Description, o => o.GetPublishingPageDescription())
                                           .ShouldBeEndOf(m => m.PageLayoutFileName, o => o.GetPublishingPagePageLayoutFileName())
                                           .ShouldBeEqual(m => m.Title, o => o.GetTitle());

            if (!string.IsNullOrEmpty(definition.Description))
            {
                assert.ShouldBeEqual(m => m.Description, o => o.GetPublishingPageDescription());
            }
            else
            {
                assert.SkipProperty(m => m.Description, "Description is NULL. Skipping.");
            }


            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert.ShouldBeEqual(m => m.ContentTypeName, o => o.GetContentTypeName());
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is NULL. Skipping.");
            }
        }

        #endregion
    }


}
