using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class DocumentParserModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DocumentParserDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelhost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DocumentParserDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(farmModelhost, farmModelhost.HostFarm, definition);
        }

        protected SPDocumentParser GetCurrentObject(SPFarm farm, DocumentParserDefinition definition)
        {
            var currentExtension = definition.FileExtension;

            var service = SPWebService.ContentService;
            var parsers = service.PluggableParsers;

            SPDocumentParser currentParser = null;

            if (parsers.ContainsKey(currentExtension))
                return parsers[currentExtension];

            return null;
        }

        private void DeployDefinition(object modelHost, SPFarm farm, DocumentParserDefinition definition)
        {
            var service = SPWebService.ContentService;
            var parsers = service.PluggableParsers;

            var currentExtension = definition.FileExtension;
            var currentParser = GetCurrentObject(farm, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentParser,
                ObjectType = typeof(SPDocumentParser),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentParser != null)
            {
                parsers.Remove(currentExtension);
                service.Update();
            }

            currentParser = new SPDocumentParser(definition.ProgId, currentExtension);
            service.PluggableParsers.Add(currentExtension, currentParser);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentParser,
                ObjectType = typeof(SPDocumentParser),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            service.Update();
        }

        #endregion
    }
}
