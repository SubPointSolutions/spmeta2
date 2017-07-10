using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;

using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using System.IO;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SuiteBarModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SuiteBarDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<SuiteBarDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, typedModelHost, typedDefinition);
        }

        private void DeployDefinition(object modelHost,
            WebApplicationModelHost typedModelHost,
            SuiteBarDefinition typedDefinition)
        {
            var currentObject = typedModelHost.HostWebApplication;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(SPWebApplication),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            MapObject(currentObject, typedDefinition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(SPWebApplication),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            typedModelHost.HostWebApplication.Update();
        }

        private void MapObject(SPWebApplication currentObject, SuiteBarDefinition typedDefinition)
        {
#if !NET35
            currentObject.SuiteBarBrandingElementHtml = typedDefinition.SuiteBarBrandingElementHtml;
#endif
        }

        #endregion
    }
}
