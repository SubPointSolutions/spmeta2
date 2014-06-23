using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.CSOM.DefaultSyntax
{
    public static class WebtDefinitionSyntax
    {
        #region behavior support

        public static ModelNode OnCreating(this ModelNode model, Action<WebDefinition, Web> action)
        {
            model.RegisterModelEvent<WebDefinition, Web>(SPMeta2.Common.ModelEventType.OnUpdating, action);

            return model;
        }

        public static ModelNode OnCreated(this ModelNode model, Action<WebDefinition, Web> action)
        {
            model.RegisterModelEvent<WebDefinition, Web>(SPMeta2.Common.ModelEventType.OnUpdated, action);

            return model;
        }

        #endregion
    }
}
