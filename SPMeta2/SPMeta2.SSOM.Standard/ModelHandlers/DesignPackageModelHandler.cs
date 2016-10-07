using System;
using System.Linq;
using Microsoft.Office.DocumentManagement;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using Microsoft.Office.Server.UserProfiles;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class DesignPackageModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DesignPackageDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DesignPackageDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, siteModelHost, definition);
        }

        private void DeployDefinition(object modelHost, SiteModelHost siteModelHost, DesignPackageDefinition definition)
        {
            var site = siteModelHost.HostSite;

            // TODO, implementation 
            // Add DesignPackage provision support #166
            // https://github.com/SubPointSolutions/spmeta2/issues/166

            DesignPackageDefinition currrentArtifact = null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currrentArtifact,
                ObjectType = typeof(DesignPackageDefinition),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            // TODO, implementation 

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currrentArtifact,
                ObjectType = typeof(DesignPackageDefinition),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
