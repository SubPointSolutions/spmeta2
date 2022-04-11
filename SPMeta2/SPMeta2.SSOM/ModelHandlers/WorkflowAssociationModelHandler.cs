using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Workflow;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Enumerations;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WorkflowAssociationModelHandler : SSOMModelHandlerBase
    {
        #region propeties
        public override Type TargetType
        {
            get { return typeof(WorkflowAssociationDefinition); }
        }
        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var workflowAssociationModel = model.WithAssertAndCast<WorkflowAssociationDefinition>("model", value => value.RequireNotNull());

            if (modelHost is ListModelHost)
            {
                var listModelHost = (modelHost as ListModelHost);
                var list = listModelHost.HostList;

                DeployListWorkflowAssociationDefinition(listModelHost, list, workflowAssociationModel);
            }
            else if (modelHost is WebModelHost)
            {
                var webModelHost = (modelHost as WebModelHost);
                var web = webModelHost.HostWeb;

                DeployWebWorkflowAssociationDefinition(webModelHost, web, workflowAssociationModel);
            }
            else if (modelHost is SPContentType)
            {
                var contentType = (modelHost as SPContentType);

                DeployContentTypeWorkflowAssociationDefinition(contentType, contentType, workflowAssociationModel);
            }
            else if (modelHost is ContentTypeLinkModelHost)
            {
                var contentTypeLinkMpodelHost = (modelHost as ContentTypeLinkModelHost);
                var contentType = contentTypeLinkMpodelHost.HostContentType;

                DeployContentTypeWorkflowAssociationDefinition(contentType, contentType, workflowAssociationModel);

                contentTypeLinkMpodelHost.ShouldUpdateHost = false;
            }
            else if (modelHost is ContentTypeModelHost)
            {
                var contentType = (modelHost as ContentTypeModelHost).HostContentType;

                DeployContentTypeWorkflowAssociationDefinition(contentType, contentType, workflowAssociationModel);
            }
            else
            {
                throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
            }
        }

        private SPWeb GetWebFromModelHost(object modelHost)
        {
            if (modelHost is ListModelHost)
            {
                return (modelHost as ListModelHost).HostList.ParentWeb;
            }

            if (modelHost is WebModelHost)
            {
                return (modelHost as WebModelHost).HostWeb;
            }


            if (modelHost is SPContentType)
            {
                return (modelHost as SPContentType).ParentWeb;
            }


            if (modelHost is ContentTypeModelHost)
            {
                return (modelHost as ContentTypeModelHost).HostWeb;
            }

            if (modelHost is ContentTypeLinkModelHost)
            {
                return (modelHost as ContentTypeLinkModelHost).HostWeb;
            }

            throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
        }

        protected SPWorkflowAssociation FindExistringWorkflowAssotiation(object modelHost, WorkflowAssociationDefinition def)
        {
            if (modelHost is ListModelHost)
            {
                var list = (modelHost as ListModelHost).HostList;

                return list.WorkflowAssociations
                          .GetAssociationByName(def.Name, list.ParentWeb.UICulture);
            }
            else if (modelHost is WebModelHost)
            {
                var web = (modelHost as WebModelHost).HostWeb;

                return web.WorkflowAssociations
                          .GetAssociationByName(def.Name, web.UICulture);
            }
            else if (modelHost is SPContentType)
            {
                var contentType = (modelHost as SPContentType);
                var web = contentType.ParentWeb;

                return contentType.WorkflowAssociations
                                   .GetAssociationByName(def.Name, web.UICulture);
            }
            else if (modelHost is ContentTypeLinkModelHost)
            {
                var listContentTypeHost = (modelHost as ContentTypeLinkModelHost);

                // don't update content type link within list
                if (listContentTypeHost.HostList != null)
                    listContentTypeHost.ShouldUpdateHost = false;

                var contentType = listContentTypeHost.HostContentType;
                var web = contentType.ParentWeb;

                return contentType.WorkflowAssociations
                                  .GetAssociationByName(def.Name, web.UICulture);
            }
            else if (modelHost is ContentTypeModelHost)
            {
                var contentType = (modelHost as ContentTypeModelHost).HostContentType;
                var web = contentType.ParentWeb;

                return contentType.WorkflowAssociations
                                  .GetAssociationByName(def.Name, web.UICulture);
            }
            else
            {
                throw new SPMeta2NotImplementedException(
                    string.Format("Unsupported model host: WorkflowAssociation under {0} is not implemented yet",
                        modelHost.GetType()));
            }
        }

        private SPWorkflowTemplate GetWorkflowTemplate(object modelHost, WorkflowAssociationDefinition def)
        {
            var targetWeb = GetWebFromModelHost(modelHost);
            return targetWeb.WorkflowTemplates.GetTemplateByName(def.WorkflowTemplateName, targetWeb.UICulture);
        }

        private void DeployContentTypeWorkflowAssociationDefinition(SPContentType modelHost, SPContentType contentType, WorkflowAssociationDefinition definition)
        {
            var existingWorkflowAssotiation = FindExistringWorkflowAssotiation(modelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(SPWorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            bool isNew = false;

            if (existingWorkflowAssotiation == null)
            {
                var workflowTemplate = GetWorkflowTemplate(modelHost, definition);

                if (workflowTemplate == null)
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find workflow template by definition:[{0}]", definition));
                }

                existingWorkflowAssotiation = SPWorkflowAssociation.CreateWebContentTypeAssociation(workflowTemplate,
                           definition.Name,
                           definition.TaskListTitle,
                           definition.HistoryListTitle);

                isNew = true;
            }

            MapProperties(definition, existingWorkflowAssotiation);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(SPWorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (isNew)
            {
                contentType.WorkflowAssociations.Add(existingWorkflowAssotiation);
                contentType.UpdateWorkflowAssociationsOnChildren(false,
                                                                 true,
                                                                 true,
                                                                 false);
            }
            else
            {
                contentType.WorkflowAssociations.Update(existingWorkflowAssotiation);
                contentType.UpdateWorkflowAssociationsOnChildren(false,
                                                                 true,
                                                                 true,
                                                                 false);
            }
        }

        private void DeployWebWorkflowAssociationDefinition(WebModelHost modelHost, Microsoft.SharePoint.SPWeb web, WorkflowAssociationDefinition definition)
        {
            var existingWorkflowAssotiation = FindExistringWorkflowAssotiation(modelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(SPWorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            bool isNew = false;

            if (existingWorkflowAssotiation == null)
            {
                var workflowTemplate = GetWorkflowTemplate(modelHost, definition);

                if (workflowTemplate == null)
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find workflow template by definition:[{0}]", definition));
                }

                existingWorkflowAssotiation = SPWorkflowAssociation.CreateListAssociation(workflowTemplate,
                           definition.Name,
                           web.Lists[definition.TaskListTitle],
                           web.Lists[definition.HistoryListTitle]);

                isNew = true;
            }

            MapProperties(definition, existingWorkflowAssotiation);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(SPWorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (isNew)
            {
                web.WorkflowAssociations.Add(existingWorkflowAssotiation);
                web.Update();
            }
            else
            {
                web.WorkflowAssociations.Update(existingWorkflowAssotiation);
            }
        }

        private void DeployListWorkflowAssociationDefinition(ListModelHost modelHost, SPList list, WorkflowAssociationDefinition definition)
        {
            var existingWorkflowAssotiation = FindExistringWorkflowAssotiation(modelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(SPWorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            bool isNew = false;

            if (existingWorkflowAssotiation == null)
            {
                var workflowTemplate = GetWorkflowTemplate(modelHost, definition);

                if (workflowTemplate == null)
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find workflow template by definition:[{0}]", definition));
                }

                existingWorkflowAssotiation = SPWorkflowAssociation.CreateListAssociation(workflowTemplate,
                           definition.Name,
                           list.ParentWeb.Lists[definition.TaskListTitle],
                           list.ParentWeb.Lists[definition.HistoryListTitle]);

                isNew = true;
            }

            MapProperties(definition, existingWorkflowAssotiation);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(SPWorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (isNew)
            {
                list.WorkflowAssociations.Add(existingWorkflowAssotiation);
                list.Update();
            }
            else
            {
                list.WorkflowAssociations.Update(existingWorkflowAssotiation);
            }
        }

        private void MapProperties(WorkflowAssociationDefinition definition, SPWorkflowAssociation association)
        {
            if (!string.IsNullOrEmpty(definition.Description))
                association.Description = definition.Description;

            if (!string.IsNullOrEmpty(definition.AssociationData))
                association.AssociationData = definition.AssociationData;

            if (definition.AllowManual.HasValue)
                association.AllowManual = definition.AllowManual.Value;

            if (definition.AutoStartChange.HasValue)
                association.AutoStartChange = definition.AutoStartChange.Value;

            if (definition.AutoStartCreate.HasValue)
                association.AutoStartCreate = definition.AutoStartCreate.Value;

            if (definition.Enabled.HasValue)
                association.Enabled = definition.Enabled.Value;
        }

        #endregion
    }
}
