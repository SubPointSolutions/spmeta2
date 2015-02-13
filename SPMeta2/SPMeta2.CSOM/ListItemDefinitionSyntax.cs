using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.CSOM
{
    [Obsolete]
    public static class ListItemDefinitionSyntax
    {
        #region methods

        public static ModelNode OnCreating(this ModelNode model, Action<ListItemDefinition, ListItem> action)
        {
            model.RegisterModelEvent<ListItemDefinition, ListItem>(SPMeta2.Common.ModelEventType.OnUpdating, action);

            return model;
        }

        public static ModelNode OnCreated(this ModelNode model, Action<ListItemDefinition, ListItem> action)
        {
            model.RegisterModelEvent<ListItemDefinition, ListItem>(SPMeta2.Common.ModelEventType.OnUpdated, action);

            return model;
        }

        #endregion
    }
}
