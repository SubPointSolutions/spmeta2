using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SupportedUICultureModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SupportedUICultureDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModel = model.WithAssertAndCast<SupportedUICultureDefinition>("model", value => value.RequireNotNull());
            var typedHost = modelHost.WithAssertAndCast<WebModelHost>("model", value => value.RequireNotNull());

            var web = typedHost.HostWeb;

            var shouldUpdateWeb = false;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = typedModel,
                ModelHost = modelHost
            });

            var currentLanguages = web.SupportedUICultures;

            if (currentLanguages.All(i => i.LCID != typedModel.LCID))
            {
                if (!web.IsMultilingual)
                    web.IsMultilingual = true;

                // add check on global installed languages
                var newCulture = new CultureInfo(typedModel.LCID);
                web.AddSupportedUICulture(newCulture);

                shouldUpdateWeb = true;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = typedModel,
                ModelHost = modelHost
            });

            if (shouldUpdateWeb)
                web.Update();
        }

        #endregion
    }
}
