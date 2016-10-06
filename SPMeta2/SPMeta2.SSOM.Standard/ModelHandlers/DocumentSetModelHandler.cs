using System;
using System.Linq;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHandlers.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using Microsoft.Office.DocumentManagement.DocumentSets;
using System.Collections;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class DocumentSetModelHandler : SSOMModelHandlerBase
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
            var typedModelHost = modelHost.WithAssertAndCast<SSOMModelHostBase>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DocumentSetDefinition>("model", value => value.RequireNotNull());

            if (typedModelHost is ListModelHost)
            {
                var folder = (typedModelHost as ListModelHost).HostList.RootFolder;

                DeployArtifact(typedModelHost, folder, definition);
            }
            else if (typedModelHost is FolderModelHost)
            {
                var folder = (typedModelHost as FolderModelHost).CurrentLibraryFolder;

                DeployArtifact(typedModelHost, folder, definition);
            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException(
                    string.Format("Model host sould be ListModelHost/FolderModelHost. Current:[{0}]", modelHost));
            }
        }

        private void DeployArtifact(SSOMModelHostBase modelHost, SPFolder folder, DocumentSetDefinition definition)
        {
            var currentDocumentSet = GetExistingDocumentSet(modelHost, folder, definition);

            var web = folder.ParentWeb;
            var documentSetName = definition.Name;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentDocumentSet,
                ObjectType = typeof(SPFolder),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentDocumentSet == null)
            {
                var contentType = GetContentType(web, definition);

                var props = new Hashtable();
                var docSet = DocumentSet.Create(folder, documentSetName, contentType.Id, props);

                currentDocumentSet = docSet.Folder;
            }

            if (!string.IsNullOrEmpty(definition.Description))
            {
                currentDocumentSet.Item["DocumentSetDescription"] = definition.Description;
                currentDocumentSet.Item.Update();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentDocumentSet,
                ObjectType = typeof(SPFolder),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        protected virtual SPContentType GetContentType(SPWeb web, DocumentSetDefinition definition)
        {
            var contentTypeName = definition.ContentTypeName;
            var contentTypeId = definition.ContentTypeId;

            // by ID, by Name
            SPContentType contentType = null;

            if (!string.IsNullOrEmpty(contentTypeId))
            {
                contentType = web.AvailableContentTypes[new SPContentTypeId(contentTypeId)];

                if (contentType == null)
                    throw new SPMeta2Exception(string.Format("Cannot find content type by ID:[{0}]", contentTypeId));
            }
            else if (!string.IsNullOrEmpty(contentTypeName))
            {
                contentType = web.AvailableContentTypes
                                 .OfType<SPContentType>()
                                 .FirstOrDefault(f => f.Name.ToUpper() == contentTypeName.ToUpper());

                if (contentType == null)
                    throw new SPMeta2Exception(string.Format("Cannot find content type by Name:[{0}]", contentTypeName));
            }

            return contentType;
        }

        protected virtual SPFolder GetExistingDocumentSet(object modelHost, SPFolder folder, DocumentSetDefinition definition)
        {
            var folderName = definition.Name;
            var parentFolder = folder;

            // dirty stuff, needs to be rewritten
            var currentFolder = parentFolder
                                   .SubFolders
                                   .OfType<SPFolder>()
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
