using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientFolderDefinitionValidator : FolderModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var folderModel = model.WithAssertAndCast<FolderDefinition>("model", value => value.RequireNotNull());

            if (ShouldDeployLibraryFolder(folderModelHost))
                ValidateLibraryFolder(folderModelHost, folderModel);
            else if (ShouldDeployListFolder(folderModelHost))
                ValidateListFolder(folderModelHost, folderModel);
        }

        private void ValidateListFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var folder = GetListFolder(folderModelHost, folderModel);

            ValidateFolderProps(folder, folderModel);
        }

        private void ValidateFolderProps(Folder folder, FolderDefinition folderModel)
        {
            TraceUtils.WithScope(traceScope =>
            {
                var pair = new ComparePair<FolderDefinition, Folder>(folderModel, folder);

                traceScope.WriteLine(string.Format("Validating model:[{0}] folder:[{1}]", folderModel, folder));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Name, o => o.Name));
            });
        }

        private void ValidateLibraryFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var folder = GetLibraryFolder(folderModelHost, folderModel);

            ValidateFolderProps(folder, folderModel);
        }
    }
}
