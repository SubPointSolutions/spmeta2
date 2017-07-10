using System;
using System.Data;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Workflow;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WorkflowAssociationModelHandler : CSOMModelHandlerBase
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
            else if (modelHost is ModelHostContext)
            {
                var contentType = (modelHost as ModelHostContext).ContentType;

                DeployContentTypeWorkflowAssociationDefinition(modelHost, contentType, workflowAssociationModel);
            }
            else if (modelHost is ContentTypeLinkModelHost)
            {
                var contentTypeLinkModelHost = (modelHost as ContentTypeLinkModelHost);

                // don't update content type link within list
                if (contentTypeLinkModelHost.HostList != null)
                    contentTypeLinkModelHost.ShouldUpdateHost = false;

                var contentType = contentTypeLinkModelHost.HostContentType;
                var web = contentTypeLinkModelHost.HostWeb;

                if (contentTypeLinkModelHost.HostList != null)
                    contentTypeLinkModelHost.ShouldUpdateHost = false;

                DeployContentTypeWorkflowAssociationDefinition(modelHost, contentType, workflowAssociationModel);
            }
            else
            {
                throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
            }
        }


        private Web GetWebFromModelHost(object modelHost)
        {
            if (modelHost is ListModelHost)
            {
                return (modelHost as ListModelHost).HostList.ParentWeb;
            }

            if (modelHost is WebModelHost)
            {
                return (modelHost as WebModelHost).HostWeb;
            }

            if (modelHost is ModelHostContext)
            {
                return (modelHost as ModelHostContext).Web;
            }

            if (modelHost is ContentTypeLinkModelHost)
            {
                var listContentTypeHost = (modelHost as ContentTypeLinkModelHost);


                return listContentTypeHost.HostWeb;
            }

            throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
        }

        protected WorkflowAssociation FindExistringWorkflowAssotiation(object modelHost, WorkflowAssociationDefinition def)
        {
            if (modelHost is ListModelHost)
            {
                var list = (modelHost as ListModelHost).HostList;
                var context = list.Context;

                var defName = def.Name;

                var res = context.LoadQuery(list.WorkflowAssociations.Where(w => w.Name == defName));
                context.ExecuteQueryWithTrace();

                return res.FirstOrDefault();
            }

            if (modelHost is WebModelHost)
            {
                var web = (modelHost as WebModelHost).HostWeb;
                var context = web.Context;

                var defName = def.Name;

                var res = context.LoadQuery(web.WorkflowAssociations.Where(w => w.Name == defName));
                context.ExecuteQueryWithTrace();

                return res.FirstOrDefault();
            }

            if (modelHost is ModelHostContext)
            {
                var contentType = (modelHost as ModelHostContext).ContentType;
                var context = contentType.Context;

                var defName = def.Name;

                var res = context.LoadQuery(contentType.WorkflowAssociations.Where(w => w.Name == defName));
                context.ExecuteQueryWithTrace();

                return res.FirstOrDefault();
            }

            if (modelHost is ContentTypeLinkModelHost)
            {
                var list = (modelHost as ContentTypeLinkModelHost).HostList;
                var context = list.Context;

                var defName = def.Name;

                var res = context.LoadQuery(list.WorkflowAssociations.Where(w => w.Name == defName));
                context.ExecuteQueryWithTrace();

                return res.FirstOrDefault();
            }

            throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
        }

        private WorkflowTemplate GetWorkflowTemplate(object modelHost, WorkflowAssociationDefinition def)
        {
            var targetWeb = GetWebFromModelHost(modelHost);
            var context = targetWeb.Context;

            var result = targetWeb.WorkflowTemplates.GetByName(def.WorkflowTemplateName);

            context.Load(result);
            context.ExecuteQueryWithTrace();

            return result;
        }

        private void DeployContentTypeWorkflowAssociationDefinition(object modelHost, ContentType contentType, WorkflowAssociationDefinition definition)
        {
            var context = contentType.Context;
            var web = (modelHost as ModelHostContext).Web;

            var existingWorkflowAssotiation = FindExistringWorkflowAssotiation(modelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(WorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (existingWorkflowAssotiation == null
                ||
                (existingWorkflowAssotiation.ServerObjectIsNull.HasValue &&
                 existingWorkflowAssotiation.ServerObjectIsNull.Value))
            {
                var workflowTemplate = GetWorkflowTemplate(modelHost, definition);

                if (workflowTemplate == null ||
                    (workflowTemplate.ServerObjectIsNull.HasValue && workflowTemplate.ServerObjectIsNull.Value))
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find workflow template by definition:[{0}]", definition));
                }

                var historyList = web.QueryAndGetListByTitle(definition.HistoryListTitle);
                var taskList = web.QueryAndGetListByTitle(definition.TaskListTitle);

                var newWorkflowAssotiation = contentType.WorkflowAssociations.Add(new WorkflowAssociationCreationInformation
                {
                    Name = definition.Name,
                    Template = workflowTemplate,
                    HistoryList = historyList,
                    TaskList = taskList
                });

                MapProperties(definition, newWorkflowAssotiation);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newWorkflowAssotiation,
                    ObjectType = typeof(WorkflowAssociation),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                if (!contentType.ReadOnly)
                {
                    contentType.Update(true);
                }

                context.ExecuteQueryWithTrace();
            }
            else
            {
                MapProperties(definition, existingWorkflowAssotiation);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingWorkflowAssotiation,
                    ObjectType = typeof(WorkflowAssociation),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                // no update
                // gives weird null ref exception

                // Enhance WorkflowAssociationDefinition - add more tests on updatability #871
                // https://github.com/SubPointSolutions/spmeta2/issues/871

                //existingWorkflowAssotiation.Update();

                if (!contentType.ReadOnly)
                {
                    contentType.Update(true);
                }

                context.ExecuteQueryWithTrace();
            }
        }


        private void DeployWebWorkflowAssociationDefinition(WebModelHost modelHost, Web web, WorkflowAssociationDefinition definition)
        {
            var context = web.Context;
            var existingWorkflowAssotiation = FindExistringWorkflowAssotiation(modelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(WorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (existingWorkflowAssotiation == null
                ||
                (existingWorkflowAssotiation.ServerObjectIsNull.HasValue &&
                 existingWorkflowAssotiation.ServerObjectIsNull.Value))
            {
                var workflowTemplate = GetWorkflowTemplate(modelHost, definition);

                if (workflowTemplate == null ||
                    (workflowTemplate.ServerObjectIsNull.HasValue && workflowTemplate.ServerObjectIsNull.Value))
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find workflow template by definition:[{0}]", definition));
                }

                var historyList = web.QueryAndGetListByTitle(definition.HistoryListTitle);
                var taskList = web.QueryAndGetListByTitle(definition.TaskListTitle);

                var newWorkflowAssotiation = web.WorkflowAssociations.Add(new WorkflowAssociationCreationInformation
                {
                    Name = definition.Name,
                    Template = workflowTemplate,
                    HistoryList = historyList,
                    TaskList = taskList
                });

                MapProperties(definition, newWorkflowAssotiation);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newWorkflowAssotiation,
                    ObjectType = typeof(WorkflowAssociation),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                web.Update();
                context.ExecuteQueryWithTrace();
            }
            else
            {
                MapProperties(definition, existingWorkflowAssotiation);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingWorkflowAssotiation,
                    ObjectType = typeof(WorkflowAssociation),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                // no update
                // gives weird null ref exception

                // Enhance WorkflowAssociationDefinition - add more tests on updatability #871
                // https://github.com/SubPointSolutions/spmeta2/issues/871

                //existingWorkflowAssotiation.Update();
                //web.Update();
                context.ExecuteQueryWithTrace();
            }
        }

        private void DeployListWorkflowAssociationDefinition(ListModelHost modelHost, List list, WorkflowAssociationDefinition definition)
        {
            var context = list.Context;
            var existingWorkflowAssotiation = FindExistringWorkflowAssotiation(modelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWorkflowAssotiation,
                ObjectType = typeof(WorkflowAssociation),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (existingWorkflowAssotiation == null
                ||
                (existingWorkflowAssotiation.ServerObjectIsNull.HasValue &&
                 existingWorkflowAssotiation.ServerObjectIsNull.Value))
            {
                var workflowTemplate = GetWorkflowTemplate(modelHost, definition);

                if (workflowTemplate == null ||
                    (workflowTemplate.ServerObjectIsNull.HasValue && workflowTemplate.ServerObjectIsNull.Value))
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find workflow template by definition:[{0}]", definition));
                }

                var historyList = list.ParentWeb.QueryAndGetListByTitle(definition.HistoryListTitle);
                var taskList = list.ParentWeb.QueryAndGetListByTitle(definition.TaskListTitle);

                var newWorkflowAssotiation = list.WorkflowAssociations.Add(new WorkflowAssociationCreationInformation
                 {
                     Name = definition.Name,
                     Template = workflowTemplate,
                     HistoryList = historyList,
                     TaskList = taskList
                 });

                MapProperties(definition, newWorkflowAssotiation);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newWorkflowAssotiation,
                    ObjectType = typeof(WorkflowAssociation),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                newWorkflowAssotiation.Update();

                context.ExecuteQueryWithTrace();
            }
            else
            {
                MapProperties(definition, existingWorkflowAssotiation);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingWorkflowAssotiation,
                    ObjectType = typeof(WorkflowAssociation),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                existingWorkflowAssotiation.Update();
                context.ExecuteQueryWithTrace();
            }
        }

        private void MapProperties(WorkflowAssociationDefinition definition, WorkflowAssociation association)
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
