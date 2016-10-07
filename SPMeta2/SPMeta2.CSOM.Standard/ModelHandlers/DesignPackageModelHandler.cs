using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client.Publishing;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client.DocumentSet;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class DesignPackageModelHandler : CSOMModelHandlerBase
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
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHandler>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DesignPackageDefinition>("model", value => value.RequireNotNull());

            DeployArtifact(siteModelHost, definition);
        }

        private void DeployArtifact(SiteModelHandler modelHost, DesignPackageDefinition definition)
        {
            // TODO, implementation 
            // Add DesignPackage provision support #166
            // https://github.com/SubPointSolutions/spmeta2/issues/166

            DesignPackageInfo currentArtifact = null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentArtifact,
                ObjectType = typeof(DesignPackageInfo),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            // TODO

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentArtifact,
                ObjectType = typeof(DesignPackageInfo),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
