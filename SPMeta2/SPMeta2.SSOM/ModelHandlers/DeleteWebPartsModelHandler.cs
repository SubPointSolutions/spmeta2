using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Web.UI.WebControls.WebParts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class DeleteWebPartsModelHandler : SSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(DeleteWebPartsDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DeleteWebPartsDefinition>("model", value => value.RequireNotNull());

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = host.HostFile,
                ObjectType = typeof(SPFile),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            var wpManager = host.SPLimitedWebPartManager;
            var webParts = wpManager.WebParts.OfType<WebPart>().ToList();

            ProcessWebPartDeletes(wpManager, webParts, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = host.HostFile,
                ObjectType = typeof(SPFile),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
        }

        protected virtual WebPart FindWebPartMatch(
           IEnumerable<WebPart> spWebPartDefenitions,
           WebPartMatch wpMatch)
        {
            // by title?
            if (!string.IsNullOrEmpty(wpMatch.Title))
            {
                return spWebPartDefenitions.FirstOrDefault(w => w.Title.ToUpper() == wpMatch.Title.ToUpper());
            }
            else
            {
                // TODO, more support by ID/Type later
                // https://github.com/SubPointSolutions/spmeta2/issues/432
            }

            return null;
        }

        protected virtual void ProcessWebPartDeletes(
            Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager wpManager,
            IEnumerable<WebPart> spWebPartDefenitions,
            DeleteWebPartsDefinition definition)
        {
            var webParts2Delete = new List<WebPart>();

            if (definition.WebParts.Any())
            {
                foreach (var webPartMatch in definition.WebParts)
                {
                    var currentWebPartMatch = FindWebPartMatch(spWebPartDefenitions, webPartMatch);

                    if (currentWebPartMatch != null && !webParts2Delete.Contains(currentWebPartMatch))
                        webParts2Delete.Add(currentWebPartMatch);
                }
            }
            else
            {
                webParts2Delete.AddRange(spWebPartDefenitions);
            }

            // clean up
            for (var index = 0; index < webParts2Delete.Count; index++)
                wpManager.DeleteWebPart(webParts2Delete[index]);
        }
    }
}
