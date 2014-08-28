using SPMeta2.SSOM.ModelHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;


using SPMeta2.Definitions;
namespace SPMeta2.Regression.SSOM.Validation
{
    public class PropertyDefinitionValidator : PropertyModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var properties = ExtractProperties(modelHost);
            var propertyModel = model.WithAssertAndCast<PropertyDefinition>("model", value => value.RequireNotNull());

            ValidateProperty(modelHost, properties, propertyModel);
        }

        private void ValidateProperty(object modelHost, System.Collections.Hashtable properties, PropertyDefinition propertyModel)
        {
            // TODO
        }

        #endregion
    }
}
