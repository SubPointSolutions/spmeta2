using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;

namespace SPMeta2.CSOM.DefaultSyntax
{
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
