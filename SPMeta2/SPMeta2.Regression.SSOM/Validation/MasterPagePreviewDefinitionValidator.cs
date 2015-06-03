using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Collections.Generic;
using System.Text;
using SPMeta2.SSOM.ModelHandlers.Base;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class MasterPagePreviewDefinitionValidator : ContentFileModelHandlerBase
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost",
                value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<MasterPagePreviewDefinition>("model", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.FileName, o => o.Name)
                .ShouldBeEqual(m => m.Title, o => o.Title);

        }

        public override string FileExtension
        {
            get { return "preview"; }
            set
            {
                
            }
        }

        protected override void MapProperties(object modelHost, System.Collections.Hashtable fileProperties, ContentPageDefinitionBase definition)
        {
            
        }

        public override System.Type TargetType
        {
            get { return typeof(MasterPagePreviewDefinition); }
        }
    }

}
