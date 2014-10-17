using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
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

            context.ExecuteQuery();

            if (!scope.HasException)
            {
                field = rootWeb.AvailableFields.GetById(id);
                context.Load(field);
                context.ExecuteQuery();
            }

            return field;
        }

        private void DeployListFieldLink(object modelHost, List list, ListFieldLinkDefinition listFieldLinkModel)
        {
            var web = list.ParentWeb;

            var context = list.Context;
            var fields = list.Fields;

            context.Load(fields);
            context.ExecuteQuery();

            Field existingListField = null;

            Field tmp = null;
            try
            {
                 tmp = list.Fields.GetById(listFieldLinkModel.FieldId);
                context.ExecuteQuery();
            }
            catch (Exception exception)
            {
                // TODO
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
                //var avialableField = web.AvailableFields;

                //context.Load(avialableField);
                //context.ExecuteQuery();

                var siteField = FindExistingSiteField(web, listFieldLinkModel.FieldId);

                fields.Add(siteField);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = siteField,
                    ObjectType = typeof(Field),
                    ObjectDefinition = listFieldLinkModel,
                    ModelHost = modelHost
                });

                context.ExecuteQuery();
            }
            else
            {
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
            }
        }

        #endregion
    }
}
