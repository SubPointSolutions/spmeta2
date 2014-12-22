using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentDatabaseModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentDatabaseDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentDatabaseDefinition>("model", value => value.RequireNotNull());

            DeployContentDatabase(webAppModelHost, webAppModelHost.HostWebApplication, definition);
        }

        protected SPContentDatabase GetCurrentContentDatabase(SPWebApplication webApp, ContentDatabaseDefinition definition)
        {
            var dbName = definition.DbName.ToUpper();

            return webApp.ContentDatabases.OfType<SPContentDatabase>()
                                          .FirstOrDefault(d => !string.IsNullOrEmpty(d.Name) && d.Name.ToUpper() == dbName);
        }

        private void DeployContentDatabase(WebApplicationModelHost modelHost, SPWebApplication webApp, ContentDatabaseDefinition definition)
        {
            var existringDb = GetCurrentContentDatabase(webApp, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existringDb,
                ObjectType = typeof(SPContentDatabase),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (existringDb == null)
            {
                existringDb = webApp.ContentDatabases.Add(
                    definition.ServerName,
                    definition.DbName,
                    definition.UserName,
                    definition.UserPassword,
                    definition.WarningSiteCollectionNumber,
                    definition.MaximumSiteCollectionNumber,
                    definition.Status);

                existringDb.Update(true);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existringDb,
                ObjectType = typeof(SPContentDatabase),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
