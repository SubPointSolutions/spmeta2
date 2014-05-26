using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class PropertyModelHandler : ModelHandlerBase
    {
        #region properties


        public override Type TargetType
        {
            get { return typeof(PropertyDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var propertyHost = modelHost.WithAssertAndCast<PropertyModelHost>("modelHost", value => value.RequireNotNull());
            var property = model.WithAssertAndCast<PropertyDefinition>("model", value => value.RequireNotNull());

            ProcessProperty(propertyHost, property);
        }

        private void ProcessProperty(PropertyModelHost host, PropertyDefinition property)
        {
            if (host.CurrentListItem != null)
                ProcessListItemProperty(host.CurrentListItem, property);
            else if (host.CurrentList != null)
                ProcessListProperty(host.CurrentList, property);
            else if (host.CurrentWeb != null)
                ProcessWebProperty(host.CurrentWeb, property);
        }

        private void ProcessWebProperty(Microsoft.SharePoint.Client.Web web, Definitions.PropertyDefinition property)
        {
            web.AllProperties[property.Key] = property.Value;
        }

        private void ProcessListProperty(Microsoft.SharePoint.Client.List list, Definitions.PropertyDefinition property)
        {
            list.RootFolder.Properties[property.Key] = property.Value;
        }

        private void ProcessListItemProperty(Microsoft.SharePoint.Client.ListItem listItem, Definitions.PropertyDefinition property)
        {
            listItem[property.Key] = property.Value;
        }

        #endregion
    }
}
