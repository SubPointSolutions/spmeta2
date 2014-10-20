using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    /// <summary>
    /// Allows to define and deploy SharePoint Image Rendition.
    /// </summary>
    public class ImageRenditionModelHandler : SSOMModelHandlerBase
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
            var imageRenditionModel = model.WithAssertAndCast<ImageRenditionDefinition>("model", value => value.RequireNotNull());

            DeployImageRenditionSettings(modelHost, siteModelHost, imageRenditionModel);
        }

        protected ImageRendition GetCurrentImageRendition(ImageRenditionCollection renditions, ImageRenditionDefinition imageRenditionModel)
        {
            return renditions.FirstOrDefault(r =>
               !string.IsNullOrEmpty(r.Name) &&
               String.Equals(r.Name, imageRenditionModel.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        private void DeployImageRenditionSettings(object modelHost, SiteModelHost siteModelHost,
            ImageRenditionDefinition imageRenditionModel)
        {
            var renditions = SiteImageRenditions.GetRenditions(siteModelHost.HostSite);
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
                currentRendition = new ImageRendition
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
                    Object = currentRendition,
                    ObjectType = typeof(ImageRendition),
                    ObjectDefinition = imageRenditionModel,
                    ModelHost = modelHost
                });

                renditions.Add(currentRendition);
                renditions.Update();
            }
            else
            {
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
