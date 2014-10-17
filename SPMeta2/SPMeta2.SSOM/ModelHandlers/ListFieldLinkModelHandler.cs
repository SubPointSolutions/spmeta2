using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListFieldLinkModelHandler : SSOMModelHandlerBase
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

        private void DeployListFieldLink(object modelHost, SPList list, ListFieldLinkDefinition listFieldLinkModel)
        {
            var existingListField = list.Fields
                                        .OfType<SPField>()
                                        .FirstOrDefault(f => f.Id == listFieldLinkModel.FieldId);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingListField,
                ObjectType = typeof(SPField),
                ObjectDefinition = listFieldLinkModel,
                ModelHost = modelHost
            });

            if (existingListField == null)
            {
                var siteField = list.ParentWeb.AvailableFields[listFieldLinkModel.FieldId];
                list.Fields.Add(siteField);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = siteField,
                    ObjectType = typeof(SPField),
                    ObjectDefinition = listFieldLinkModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingListField,
                    ObjectType = typeof(SPField),
                    ObjectDefinition = listFieldLinkModel,
                    ModelHost = modelHost
                });
            }
        }

        #endregion
    }
}
