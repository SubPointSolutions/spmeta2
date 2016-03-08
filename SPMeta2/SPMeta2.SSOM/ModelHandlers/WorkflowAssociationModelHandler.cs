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

            if (modelHost is WebModelHost)
            {
                throw new SPMeta2NotImplementedException("todo");
            }


            throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
        }

        private SPWorkflowTemplate GetWorkflowTemplate(object modelHost, WorkflowAssociationDefinition def)
        {
            var targetWeb = GetWebFromModelHost(modelHost);
            return targetWeb.WorkflowTemplates.GetTemplateByName(def.WorkflowTemplateName, targetWeb.UICulture);
        }

        private void DeployWebWorkflowAssociationDefinition(WebModelHost webModelHost, Microsoft.SharePoint.SPWeb web, WorkflowAssociationDefinition workflowAssociationModel)
        {
            // TODO
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

                existingWorkflowAssotiation = SPWorkflowAssociation.CreateListAssociation(workflowTemplate,
                           definition.Name,
                           list.Lists[definition.TaskListTitle],
                           list.Lists[definition.HistoryListTitle]);

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
            }
            else
            {
                // ??
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
