using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListFieldLinkModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListFieldLinkDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listFieldLinkModel = model.WithAssertAndCast<ListFieldLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            DeployListFieldLink(modelHost, list, listFieldLinkModel);
        }

        private Field FindExistingSiteField(Web rootWeb, Guid id)
        {
            var context = rootWeb.Context;
            var scope = new ExceptionHandlingScope(context);

            Field field = null;

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    rootWeb.AvailableFields.GetById(id);
                }

                using (scope.StartCatch())
                {

                }
            }

            context.ExecuteQueryWithTrace();

            if (!scope.HasException)
            {
                field = rootWeb.AvailableFields.GetById(id);

                context.Load(field);
                context.Load(field, f => f.SchemaXmlWithResourceTokens);
                context.Load(field, f => f.SchemaXml);

                context.ExecuteQueryWithTrace();
            }

            return field;
        }

        private void DeployListFieldLink(object modelHost, List list, ListFieldLinkDefinition listFieldLinkModel)
        {
            var web = list.ParentWeb;

            var context = list.Context;
            var fields = list.Fields;

            context.Load(fields);
            context.ExecuteQueryWithTrace();

            Field existingListField = null;

            Field tmp = null;
            try
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Fetching site field by ID: [{0}]", listFieldLinkModel.FieldId);

                tmp = list.Fields.GetById(listFieldLinkModel.FieldId);
                context.ExecuteQueryWithTrace();
            }
            catch (Exception exception)
            {
                TraceService.ErrorFormat((int)LogEventId.ModelProvisionCoreCall, "Cannot find site field by ID: [{0}]. Provision might break.", listFieldLinkModel.FieldId);
            }
            finally
            {
                if (tmp != null && tmp.ServerObjectIsNull.HasValue && !tmp.ServerObjectIsNull.Value)
                    existingListField = tmp;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingListField,
                ObjectType = typeof(Field),
                ObjectDefinition = listFieldLinkModel,
                ModelHost = modelHost
            });

            if (existingListField == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list field");

                var siteField = FindExistingSiteField(web, listFieldLinkModel.FieldId);

                var addFieldOptions = (AddFieldOptions)(int)listFieldLinkModel.AddFieldOptions;

                Field listField = null;

                // AddToDefaultView || !DefaultValue would use AddFieldAsXml
                // this would change InternalName of the field inside the list

                // The second case, fields.Add(siteField), does not change internal name of the field inside list
                if (addFieldOptions != AddFieldOptions.DefaultValue || listFieldLinkModel.AddToDefaultView)
                    listField = fields.AddFieldAsXml(siteField.SchemaXmlWithResourceTokens, listFieldLinkModel.AddToDefaultView, addFieldOptions);
                else
                    listField = fields.Add(siteField);

                ProcessListFieldLinkProperties(listField, listFieldLinkModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = listField,
                    ObjectType = typeof(Field),
                    ObjectDefinition = listFieldLinkModel,
                    ModelHost = modelHost
                });

                listField.Update();
                context.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list field");

                ProcessListFieldLinkProperties(existingListField, listFieldLinkModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingListField,
                    ObjectType = typeof(Field),
                    ObjectDefinition = listFieldLinkModel,
                    ModelHost = modelHost
                });

                existingListField.Update();
                context.ExecuteQueryWithTrace();
            }
        }

        private void ProcessListFieldLinkProperties(Field existingListField, ListFieldLinkDefinition listFieldLinkModel)
        {
            if (!string.IsNullOrEmpty(listFieldLinkModel.DisplayName))
                existingListField.Title = listFieldLinkModel.DisplayName;

            if (listFieldLinkModel.Hidden.HasValue)
                existingListField.Hidden = listFieldLinkModel.Hidden.Value;

            if (listFieldLinkModel.Required.HasValue)
                existingListField.Required = listFieldLinkModel.Required.Value;
        }

        #endregion
    }
}
