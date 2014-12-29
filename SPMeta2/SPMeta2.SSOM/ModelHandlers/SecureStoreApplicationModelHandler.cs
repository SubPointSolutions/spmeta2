using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.SecureStoreService.Server;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SecureStoreApplicationModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SecureStoreApplicationDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmAppModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SecureStoreApplicationDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, farmAppModelHost.HostFarm, definition);
        }

        public override void WithResolvingModelHost(ModelHostResolveContext context)
        {
            var farmAppModelHost = context.ModelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = context.Model.WithAssertAndCast<SecureStoreApplicationDefinition>("model", value => value.RequireNotNull());

            var currentObject = GetCurrentSecureStoreApplication(farmAppModelHost.HostFarm, definition);

            context.Action(new SecureStoreApplicationModelHost
            {
                HostFarm = farmAppModelHost.HostFarm,
                HostSecureStore = currentObject
            });
        }

        protected ISecureStore GetCurrentSecureStoreApplication(SPFarm spFarm, SecureStoreApplicationDefinition definition)
        {
            if (definition.UseDefault)
            {
                var context = SPServiceContext.GetContext(SPServiceApplicationProxyGroup.Default, SPSiteSubscriptionIdentifier.Default);
                var ssp = new SecureStoreServiceProxy();
                return ssp.GetSecureStore(context);
            }
            else
            {
                throw new SPMeta2NotImplementedException("Secure Store resolution by Name/Id is not supported yet. Please use 'UseDefault' property set 'true' instead.");
            }
        }

        private void DeployDefinition(object modelHost, SPFarm spFarm, SecureStoreApplicationDefinition definition)
        {
            var currentObject = GetCurrentSecureStoreApplication(spFarm, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(ISecureStore),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });


            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(ISecureStore),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
