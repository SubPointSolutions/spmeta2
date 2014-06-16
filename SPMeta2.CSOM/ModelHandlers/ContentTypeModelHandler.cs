using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
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
                var context = site.Context;
                var rootWeb = site.RootWeb;

                context.Load(rootWeb, w => w.ContentTypes);
                context.Load(rootWeb, w => w.ServerRelativeUrl);

                context.ExecuteQuery();

                var currentContentType = rootWeb.ContentTypes.FindByName(contentTypeModel.Name);

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

                // doublt update to promote change tpo the list 
                // have no idea why it does not work from the first time

                currentContentType.Update(true);
                context.ExecuteQuery();
            }
            else
            {
                action(modelHost);
            }
        }


        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
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

            InvokeOnModelEvents<ContentTypeDefinition, ContentType>(null, ModelEventType.OnUpdating);

            //var currentContentType = rootWeb.ContentTypes.FindByName(contentTypeModel.Name);
            var contentTypeId = contentTypeModel.GetContentTypeId();
            var currentContentType = contentTypes.FirstOrDefault(c => c.StringId.ToLower() == contentTypeId.ToLower());

            if (currentContentType == null)
            {
                // here we have 2 cases
                // (1) OOTB parent content types might be resolved by ID
                // (2) custom content types have to be resolved by NAME as id is always different :(
                //var parentContentType = rootWeb.ContentTypes.GetById(contentTypeModel.ParentContentTypeId);

                currentContentType = rootWeb.ContentTypes.Add(new ContentTypeCreationInformation
                {
                    //ParentContentType = parentContentType,
                    Name = contentTypeModel.Name,
                    Description = string.IsNullOrEmpty(contentTypeModel.Description) ? string.Empty : contentTypeModel.Description,
                    Group = contentTypeModel.Group,
                    Id = contentTypeId
                });
            }

            currentContentType.Name = contentTypeModel.Name;
            currentContentType.Description = string.IsNullOrEmpty(contentTypeModel.Description) ? string.Empty : contentTypeModel.Description;
            currentContentType.Group = contentTypeModel.Group;

            InvokeOnModelEvents<ContentTypeDefinition, ContentType>(currentContentType, ModelEventType.OnUpdated);

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
