using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SandboxSolutionModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SandboxSolutionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var sandboxSolutionDefinition = model.WithAssertAndCast<SandboxSolutionDefinition>("model", value => value.RequireNotNull());

            DeploySandboxSolution(modelHost, siteModelHost.HostSite, sandboxSolutionDefinition);
        }

        private void DeploySandboxSolution(object modelHost, Microsoft.SharePoint.SPSite site, SandboxSolutionDefinition sandboxSolutionDefinition)
        {
            var solutionGallery = (SPDocumentLibrary)site.GetCatalog(SPListTemplateType.SolutionCatalog);
            var file = solutionGallery.RootFolder.Files.Add(sandboxSolutionDefinition.FileName, sandboxSolutionDefinition.Content, true);

            if (sandboxSolutionDefinition.Activate)
            {
                var userSolution = site.Solutions.Add(file.Item.ID);
            }
        }

        #endregion
    }
}
