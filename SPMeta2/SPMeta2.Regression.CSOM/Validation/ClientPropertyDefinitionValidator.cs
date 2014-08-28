using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientPropertyDefinitionValidator : PropertyModelHandler
    {
        #region properties

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var properties = ExtractProperties(modelHost);
            var property = model.WithAssertAndCast<PropertyDefinition>("model", value => value.RequireNotNull());

            ValidateProperty(modelHost, properties, property);
        }

        private void ValidateProperty(object modelHost, Microsoft.SharePoint.Client.PropertyValues properties, PropertyDefinition property)
        {
            // TODO
        }

        #endregion
    }
}
