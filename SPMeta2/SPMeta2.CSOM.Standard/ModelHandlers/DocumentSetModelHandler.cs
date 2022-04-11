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
    public class DocumentSetModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DocumentSetDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DocumentSetDefinition>("model", value => value.RequireNotNull());

            if (typedModelHost is ListModelHost)
            {
                var folder = (typedModelHost as ListModelHost).HostList.RootFolder;

                DeployArtifact(typedModelHost, folder, definition);
            }
            else if (typedModelHost is FolderModelHost)
            {
                var folder = (typedModelHost as FolderModelHost).CurrentListFolder;

                DeployArtifact(typedModelHost, folder, definition);
            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException(
                    string.Format("Model host sould be ListModelHost/FolderModelHost. Current:[{0}]", modelHost));
            }
        }

        private void DeployArtifact(CSOMModelHostBase modelHost, Folder folder, DocumentSetDefinition definition)
        {
            var currentDocumentSet = GetExistingDocumentSet(modelHost, folder, definition);

            var context = modelHost.HostClientContext;
            var web = modelHost.HostWeb;

            var documentSetName = definition.Name;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentDocumentSet,
                ObjectType = typeof(Folder),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentDocumentSet == null)
            {
                var contentType = GetContentType(modelHost, definition);

                DocumentSet.Create(context, folder, documentSetName, contentType.Id);
                context.ExecuteQueryWithTrace();

                currentDocumentSet = GetExistingDocumentSet(modelHost, folder, definition);
            }

            if (!string.IsNullOrEmpty(definition.Description))
            {
                currentDocumentSet.ListItemAllFields["DocumentSetDescription"] = definition.Description;
                currentDocumentSet.ListItemAllFields.Update();

                context.ExecuteQueryWithTrace();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentDocumentSet,
                ObjectType = typeof(Folder),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        protected virtual ContentType GetContentType(CSOMModelHostBase modelHost, DocumentSetDefinition definition)
        {
            var contentTypeName = definition.ContentTypeName;
            var contentTypeId = definition.ContentTypeId;

            var context = modelHost.HostClientContext;
            var web = modelHost.HostWeb;

            ContentType contentType = null;

#if !NET35
            if (!string.IsNullOrEmpty(contentTypeId))
            {
                var tmpContentType = context.LoadQuery(web.AvailableContentTypes.Where(ct => ct.StringId == contentTypeId));
                context.ExecuteQueryWithTrace();

                contentType = tmpContentType.FirstOrDefault();

                if (contentType == null)
                    throw new SPMeta2Exception(string.Format("Cannot find content type by ID:[{0}]", contentTypeId));
            }
            else if (!string.IsNullOrEmpty(contentTypeName))
            {
                var tmpContentType = context.LoadQuery(web.AvailableContentTypes.Where(ct => ct.Name == contentTypeName));
                context.ExecuteQueryWithTrace();

                contentType = tmpContentType.FirstOrDefault();

                if (contentType == null)
                    throw new SPMeta2Exception(string.Format("Cannot find content type by Name:[{0}]", contentTypeName));
            }
#endif

#if NET35
            // SP2010 CSOM does not have an option to get the content type by ID
            // fallback to Name, and that's a huge thing all over the M2 library and provision
            
            var tmpContentType = context.LoadQuery(web.AvailableContentTypes.Where(ct => ct.Name == contentTypeName));
            context.ExecuteQueryWithTrace();

            contentType = tmpContentType.FirstOrDefault();

            if (contentType == null)
                throw new SPMeta2Exception(string.Format("Cannot find content type by Name:[{0}]", contentTypeName));
#endif

            return contentType;

        }

        protected virtual Folder GetExistingDocumentSet(CSOMModelHostBase modelHost, Folder folder, DocumentSetDefinition definition)
        {
            var folderName = definition.Name;
            var parentFolder = folder;

            var context = parentFolder.Context;

            context.Load(parentFolder, f => f.Folders);
            context.ExecuteQueryWithTrace();

            // dirty stuff, needs to be rewritten
            var currentFolder = parentFolder
                                   .Folders
                                   .OfType<Folder>()
                                   .FirstOrDefault(f => f.Name == folderName);

            if (currentFolder != null)
            {
                return currentFolder;
            }

            return null;
        }

        #endregion
    }
}
