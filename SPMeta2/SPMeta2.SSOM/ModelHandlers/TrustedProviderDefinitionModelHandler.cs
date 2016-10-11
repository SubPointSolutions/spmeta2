using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;
using System.Security;
using Microsoft.SharePoint.Administration.Claims;
using System.Security.Cryptography.X509Certificates;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class TrustedAccessProviderModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TrustedAccessProviderDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TrustedAccessProviderDefinition>("model", value => value.RequireNotNull());

            DeployAftifact(farmModelHost, farmModelHost.HostFarm, definition);
        }

        private void DeployAftifact(FarmModelHost modelHost, SPFarm spFarm, TrustedAccessProviderDefinition definition)
        {
            var currentObject = GetCurrentTrustedAccessProvider(spFarm, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(SPTrustedAccessProvider),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentObject == null)
            {
                var securityTokenService = SPSecurityTokenServiceManager.Local;

                if (securityTokenService == null)
                    throw new SPMeta2Exception("SPSecurityTokenServiceManager.Local is NULL");

                if (definition != null && definition.Certificate.Count() > 0)
                {
                    var certificate = new X509Certificate2(definition.Certificate);

                    currentObject = new SPTrustedAccessProvider(securityTokenService,
                                                                definition.Name,
                                                                definition.Description ?? string.Empty,
                                                                certificate);
                }
                else
                {
                    currentObject = new SPTrustedAccessProvider(securityTokenService,
                                                                          definition.Name,
                                                                          definition.Description ?? string.Empty);
                }
            }

#if !NET35
            if (!string.IsNullOrEmpty(definition.MetadataEndPoint))
            {
                if (currentObject.MetadataEndPoint != null)
                {
                    if (currentObject.MetadataEndPoint.AbsoluteUri.ToUpper() !=
                        new Uri(definition.MetadataEndPoint).AbsoluteUri.ToUpper())
                    {
                        currentObject.MetadataEndPoint = new Uri(definition.MetadataEndPoint);
                    }
                }
                else
                {
                    currentObject.MetadataEndPoint = new Uri(definition.MetadataEndPoint);
                }
            }
#endif

            if (definition != null && definition.Certificate.Count() > 0)
            {
                var certificate = new X509Certificate2(definition.Certificate);

                if (currentObject.SigningCertificate.Thumbprint != certificate.Thumbprint)
                {
                    currentObject.SigningCertificate = certificate;
                }
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(SPTrustedAccessProvider),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            currentObject.Update();
        }

        protected virtual SPTrustedAccessProvider GetCurrentTrustedAccessProvider(object modelHost, TrustedAccessProviderDefinition definition)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());

            return GetCurrentTrustedAccessProvider(farmModelHost.HostFarm, definition);

        }

        protected virtual SPTrustedAccessProvider GetCurrentTrustedAccessProvider(SPFarm spFarm, TrustedAccessProviderDefinition definition)
        {
            SPTrustedAccessProvider result = null;
            var securityTokenService = SPSecurityTokenServiceManager.Local;

            result = securityTokenService.TrustedAccessProviders
                                         .FirstOrDefault(p => !string.IsNullOrEmpty(p.Name)
                                                && (p.Name.ToUpper() == definition.Name.ToUpper()));

            return result;

        }

        #endregion

    }
}
