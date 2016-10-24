using System;
using System.Linq;
using Microsoft.Office.DocumentManagement;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using Microsoft.Office.Server.UserProfiles;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class CorePropertyModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(CorePropertyDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<CorePropertyDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, siteModelHost, definition);
        }

        private void DeployDefinition(object modelHost, SiteModelHost siteModelHost, CorePropertyDefinition definition)
        {
            var site = siteModelHost.HostSite;

            // TODO, implementation 
            // Add user profile property provision support #820
            // https://github.com/SubPointSolutions/spmeta2/issues/820

            CoreProperty currentProperty = GetCurrentCoreProperty(site, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentProperty,
                ObjectType = typeof(CoreProperty),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentProperty == null)
                currentProperty = CreateNewCoreProperty(site, definition);

            MapProperties(currentProperty, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentProperty,
                ObjectType = typeof(CoreProperty),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        private void MapProperties(CoreProperty currentProperty, CorePropertyDefinition definition)
        {
            if (definition.IsAlias.HasValue)
                currentProperty.IsAlias = definition.IsAlias.Value;

            if (definition.IsSearchable.HasValue)
                currentProperty.IsSearchable = definition.IsSearchable.Value;

            // this is cannot be updated
            //if (definition.IsMultivalued.HasValue)
            //    currentProperty.IsMultivalued = definition.IsMultivalued.Value;
        }

        protected virtual CoreProperty CreateNewCoreProperty(SPSite site, CorePropertyDefinition definition)
        {
            var serverContext = SPServiceContext.GetContext(site);
            var profileManager = new UserProfileManager(serverContext);

            var profilePropertiesManager = new UserProfileConfigManager(serverContext).ProfilePropertyManager;
            var corePropertiesManager = profilePropertiesManager.GetCoreProperties();

            var coreProp = corePropertiesManager.Create(false);

            coreProp.Name = definition.Name;
            coreProp.DisplayName = definition.DisplayName;

            coreProp.Type = definition.Type;

            if (!string.IsNullOrEmpty(definition.Description))
                coreProp.Description = definition.Description;

            if (definition.Length.HasValue)
                coreProp.Length = definition.Length.Value;

            if (definition.IsAlias.HasValue)
                coreProp.IsAlias = definition.IsAlias.Value;

            if (definition.IsSearchable.HasValue)
                coreProp.IsSearchable = definition.IsSearchable.Value;

            if (definition.IsMultivalued.HasValue)
                coreProp.IsMultivalued = definition.IsMultivalued.Value;

            corePropertiesManager.Add(coreProp);

            return coreProp;
        }

        protected virtual CoreProperty GetCurrentCoreProperty(SPSite site, CorePropertyDefinition definition)
        {
            CoreProperty result = null;

            var serverContext = SPServiceContext.GetContext(site);
            var profileManager = new UserProfileManager(serverContext);

            var profilePropertiesManager = new UserProfileConfigManager(serverContext).ProfilePropertyManager;
            var corePropertiesManager = profilePropertiesManager.GetCoreProperties();

            // would return NULL, no try-catch is required
            result = corePropertiesManager.GetPropertyByName(definition.Name);

            return result;
        }

        #endregion
    }
}
