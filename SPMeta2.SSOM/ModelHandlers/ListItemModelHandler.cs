using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListItemModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListItemDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = model.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            DeployInternall(list, listItemModel);
        }

        private void DeployInternall(SPList list, ListItemDefinition listItemModel)
        {
            if (IsDocumentLibray(list))
            {
                DeployDocumentLibraryItem(list, listItemModel);
            }
            else
            {
                DeployListItem(list, listItemModel);
            }
        }

        private void DeployListItem(SPList list, ListItemDefinition listItemModel)
        {
            var listItem = GetOrCreateListItemByTitle(list, listItemModel);
            UpdateListItem(listItem, listItemModel);
        }

        private void UpdateListItem(SPListItem listItem, ListItemDefinition listItemModel)
        {
            throw new NotImplementedException();

            // UPDATE. TODO, 

            if (listItemModel.UpdateOverwriteVersion)
                listItem.UpdateOverwriteVersion();
            else if (listItemModel.SystemUpdate)
                listItem.SystemUpdate(listItemModel.SystemUpdateIncrementVersionNumber);
            else
            {
                listItem.Update();
            }
        }

        private SPListItem GetOrCreateListItemByTitle(SPList list, ListItemDefinition listItemModel)
        {
            throw new NotImplementedException();
        }

        private void DeployDocumentLibraryItem(SPList list, ListItemDefinition listItemModel)
        {
            var listItem = GetOrCreateDocumentItemByName(list, listItemModel);
            UpdateDocumentItem(listItem, listItemModel);
        }

        private void UpdateDocumentItem(object listItem, ListItemDefinition listItemModel)
        {
            // TODO, handle all checking-versions-publishing stuff

            throw new NotImplementedException();
        }

        private object GetOrCreateDocumentItemByName(SPList list, ListItemDefinition listItemModel)
        {
            throw new NotImplementedException();
        }

        private bool IsDocumentLibray(SPList list)
        {
            // TODO

            return false;
        }

        #endregion
    }
}
