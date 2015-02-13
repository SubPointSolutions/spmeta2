using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.BusinessData.Infrastructure.SecureStore;
using Microsoft.Office.SecureStoreService.Server;
using Microsoft.SharePoint.Administration.Claims;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class TargetApplicationModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TargetApplicationDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var secureStoreModelHost = modelHost.WithAssertAndCast<SecureStoreApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TargetApplicationDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, secureStoreModelHost.HostSecureStore, definition);
        }

        protected TargetApplication GetCurrentObject(ISecureStore secureStore, TargetApplicationDefinition definition)
        {
            var apps = secureStore.GetApplications();

            if (!string.IsNullOrEmpty(definition.ApplicationId))
            {
                return apps.FirstOrDefault(app => app.ApplicationId.ToUpper() == definition.ApplicationId.ToUpper());
            }
            else if (!string.IsNullOrEmpty(definition.Name))
            {
                return apps.FirstOrDefault(app => app.Name.ToUpper() == definition.FriendlyName.ToUpper());
            }
            else if (!string.IsNullOrEmpty(definition.FriendlyName))
            {
                return apps.FirstOrDefault(app => app.FriendlyName.ToUpper() == definition.FriendlyName.ToUpper());
            }
            else
            {
                throw new SPMeta2Exception("ApplicationId/Name/FriendlyName needs to be defined.");
            }
        }

        private void DeployDefinition(object modelHost, ISecureStore hostSecureStore, TargetApplicationDefinition definition)
        {
            var currentObject = GetCurrentObject(hostSecureStore, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(TargetApplication),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentObject == null)
                currentObject = CreateOject(modelHost, hostSecureStore, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(TargetApplication),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        private static SecureStoreServiceClaim GetSecureStoreClaim(string userName)
        {
            var claim = SPClaimProviderManager.CreateUserClaim(userName, SPOriginalIssuerType.Windows);
            return new SecureStoreServiceClaim(claim);
        }

        private TargetApplication CreateOject(object modelHost, ISecureStore hostSecureStore, TargetApplicationDefinition definition)
        {
            var appType = (TargetApplicationType)Enum.Parse(typeof(TargetApplicationType), definition.Type);

            var secureStoreClaimns = new List<SecureStoreServiceClaim>();

            foreach (var claim in definition.TargetApplicationClams)
                secureStoreClaimns.Add(GetSecureStoreClaim(claim));

            var appClaims = new TargetApplicationClaims(secureStoreClaimns, null, null);
            var appFields = GetFields(definition);

            hostSecureStore.CreateApplication(
                new TargetApplication(definition.ApplicationId, definition.FriendlyName, definition.ContactEmail,
                    definition.TicketTimeout,
                    appType,
                    new Uri(definition.CredentialManagementUrl)),
                appFields, appClaims);

            return GetCurrentObject(hostSecureStore, definition);
        }

        private TargetApplicationField[] GetFields(TargetApplicationDefinition definition)
        {
            var result = new List<TargetApplicationField>();

            foreach (var field in definition.Fields)
            {
                result.Add(new TargetApplicationField(

                 field.Name,
                 field.IsMasked,
                 (SecureStoreCredentialType)Enum.Parse(typeof(SecureStoreCredentialType), field.CredentialType)
                ));
            }

            return result.ToArray();
        }

        #endregion

    }
}
