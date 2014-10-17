using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var contentTypeDefinition = model as ContentTypeDefinition;

            if (site != null && contentTypeDefinition != null)
            {
                var rootWeb = site.RootWeb;
                var contentTypeId = new SPContentTypeId(contentTypeDefinition.GetContentTypeId());

                // SPBug, it has to be new SPWen for every content type operation inside feature event handler
                using (var tmpRootWeb = site.OpenWeb(rootWeb.ID))
                {
                    var targetContentType = tmpRootWeb.ContentTypes[contentTypeId];

                    if (childModelType == typeof(ModuleFileDefinition))
                    {
                        action(targetContentType.ResourceFolder);
                    }
                    else
                    {
                        action(targetContentType);
                    }

                    targetContentType.Update(true);
                    tmpRootWeb.Update();
                }
            }
            else
            {
                action(modelHost);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var contentTypeModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var rootWeb = site.RootWeb;

            // SPBug, it has to be new SPWen for every content type operation inside feature event handler
            using (var tmpRootWeb = site.OpenWeb(rootWeb.ID))
            {
                var contentTypeId = new SPContentTypeId(contentTypeModel.GetContentTypeId());

                var targetContentType = tmpRootWeb.ContentTypes[contentTypeId];

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = targetContentType,
                    ObjectType = typeof(SPContentType),
                    ObjectDefinition = contentTypeModel,
                    ModelHost = modelHost
                });

                if (targetContentType == null)
                    targetContentType = tmpRootWeb
                                            .ContentTypes
                                            .Add(new SPContentType(contentTypeId, tmpRootWeb.ContentTypes, contentTypeModel.Name));


                targetContentType.Name = contentTypeModel.Name;
                targetContentType.Group = contentTypeModel.Group;

                // SPBug, description cannot be null
                targetContentType.Description = contentTypeModel.Description ?? string.Empty;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = targetContentType,
                    ObjectType = typeof(SPContentType),
                    ObjectDefinition = contentTypeModel,
                    ModelHost = modelHost
                });
                
                targetContentType.Update();

                tmpRootWeb.Update();
            }
        }

        #endregion
    }
}
