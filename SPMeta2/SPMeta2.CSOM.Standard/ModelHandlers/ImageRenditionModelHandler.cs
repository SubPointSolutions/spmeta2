using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class ImageRenditionModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ImageRenditionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var navigationModel = model.WithAssertAndCast<ImageRenditionDefinition>("model", value => value.RequireNotNull());

            DeployImageRenditionSettings(modelHost, siteModelHost, navigationModel);
        }

        protected ImageRendition GetCurrentImageRendition(IList<ImageRendition> renditions, ImageRenditionDefinition imageRenditionModel)
        {
            return renditions.FirstOrDefault(r =>
               !string.IsNullOrEmpty(r.Name) &&
               String.Equals(r.Name, imageRenditionModel.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        private void DeployImageRenditionSettings(object modelHost, SiteModelHost siteModelHost,
            ImageRenditionDefinition imageRenditionModel)
        {
            var context = siteModelHost.HostSite.Context;
            var renditions = SiteImageRenditions.GetRenditions(siteModelHost.HostSite.Context);
            context.ExecuteQueryWithTrace();

            var currentRendition = GetCurrentImageRendition(renditions, imageRenditionModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentRendition,
                ObjectType = typeof(ImageRendition),
                ObjectDefinition = imageRenditionModel,
                ModelHost = modelHost
            });

            if (currentRendition == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new Image Rendition");

                var newRendition = new ImageRendition
                 {
                     Name = imageRenditionModel.Name,
                     Width = imageRenditionModel.Width,
                     Height = imageRenditionModel.Height
                 };

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newRendition,
                    ObjectType = typeof(ImageRendition),
                    ObjectDefinition = imageRenditionModel,
                    ModelHost = modelHost
                });

                renditions.Add(newRendition);
                SiteImageRenditions.SetRenditions(context, renditions);

                context.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing existing Image Rendition");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentRendition,
                    ObjectType = typeof(ImageRendition),
                    ObjectDefinition = imageRenditionModel,
                    ModelHost = modelHost
                });
            }
        }


        #endregion
    }
}
