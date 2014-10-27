using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using System.Xml.Linq;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ContentTypeModelHandler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ContentTypeDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var contentTypeModel = model as ContentTypeDefinition;

            if (site != null && contentTypeModel != null)
            {
                var rootWeb = site.RootWeb;
                var context = rootWeb.Context;

                var id = contentTypeModel.GetContentTypeId();

                var currentContentType = rootWeb.ContentTypes.GetById(id);
                context.ExecuteQuery();

                if (childModelType == typeof(ModuleFileDefinition))
                {
                    var ctDocument = XDocument.Parse(currentContentType.SchemaXml);
                    var folderUrlNode = ctDocument.Descendants().FirstOrDefault(d => d.Name == "Folder");

                    var webRelativeFolderUrl = folderUrlNode.Attribute("TargetName").Value;
                    var serverRelativeFolderUrl = rootWeb.ServerRelativeUrl + "/" + webRelativeFolderUrl;

                    var ctFolder = rootWeb.GetFolderByServerRelativeUrl(serverRelativeFolderUrl);
                    context.ExecuteQuery();

                    action(new FolderModelHost
                    {

                        CurrentWeb = rootWeb,
                        CurrentList = null,
                        CurrentLibraryFolder = ctFolder
                    });
                }
                else
                {
                    // ModelHostContext is a cheat for client OM
                    // the issue is that having ContenType instance to work with FieldLinks is not enought - you ned RootWeb
                    // and RootWeb could be accessed only via Site
                    // so, somehow we need to pass this info to the model handler

                    action(new ModelHostContext
                    {
                        Site = site,
                        ContentType = currentContentType
                    });
                }

                currentContentType.Update(true);
                context.ExecuteQuery();
            }
            else
            {
                action(modelHost);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var contentTypeModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var rootWeb = site.RootWeb;
            var context = rootWeb.Context;

            var contentTypeId = contentTypeModel.GetContentTypeId();

          
            var tmp = rootWeb.ContentTypes.GetById(contentTypeId);
            context.ExecuteQuery();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = tmp,
                ObjectType = typeof(ContentType),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            InvokeOnModelEvent<ContentTypeDefinition, ContentType>(null, ModelEventType.OnUpdating);

            ContentType currentContentType = null;

            if (tmp == null || tmp.ServerObjectIsNull == null || tmp.ServerObjectIsNull.Value)
            {
                currentContentType = rootWeb.ContentTypes.Add(new ContentTypeCreationInformation
                {
                    Name = contentTypeModel.Name,
                    Description = string.IsNullOrEmpty(contentTypeModel.Description) ? string.Empty : contentTypeModel.Description,
                    Group = contentTypeModel.Group,
                    Id = contentTypeId
                });
            }
            else
            {
                currentContentType = tmp;
            }

            currentContentType.Name = contentTypeModel.Name;
            currentContentType.Description = string.IsNullOrEmpty(contentTypeModel.Description) ? string.Empty : contentTypeModel.Description;
            currentContentType.Group = contentTypeModel.Group;

            InvokeOnModelEvent<ContentTypeDefinition, ContentType>(currentContentType, ModelEventType.OnUpdated);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentContentType,
                ObjectType = typeof(ContentType),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            currentContentType.Update(true);
            context.ExecuteQuery();
        }

        public override void RetractModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var contentTypeModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var context = site.Context;
            var rootWeb = site.RootWeb;

            var contentTypes = rootWeb.ContentTypes;

            context.Load(rootWeb);
            context.Load(contentTypes);

            context.ExecuteQuery();

            var contentTypeId = contentTypeModel.GetContentTypeId();

            var currentContentType = contentTypes.FirstOrDefault(c => c.StringId.ToLower() == contentTypeId.ToLower());

            if (currentContentType != null)
            {
                currentContentType.DeleteObject();
                context.ExecuteQuery();
            }
        }
    }

}
