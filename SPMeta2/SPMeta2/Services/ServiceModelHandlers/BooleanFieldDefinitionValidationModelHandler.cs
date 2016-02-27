using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Exceptions;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public class BooleanFieldDefinitionValidationModelHandler : TypedDefinitionModelHandlerBase<BooleanFieldDefinition>
    {
        protected override void ProcessDefinition(object modelHost, BooleanFieldDefinition model)
        {
            // https://github.com/SubPointSolutions/spmeta2/issues/792
            // When creating a BooleanFieldDefinition you have to use "0" and "1" for DefaultValue else it won't work. 
            // Neither "false" nor false.ToString() work, so there should be an BuiltIn option for this.

            if (!string.IsNullOrEmpty(model.DefaultValue))
            {
                var isValidValue = IsValidDefaultValue(model);

                if (!isValidValue)
                {
                    throw new SPMeta2ModelValidationException(
                        string.Format("Default value should either be '0' or '1'. Current value:[{1}] Definition:[{0}]", model, model.DefaultValue));
                }
            }
        }

        protected virtual bool IsValidDefaultValue(BooleanFieldDefinition model)
        {
            var value = model.DefaultValue;

            if (string.IsNullOrEmpty(value))
                return true;

            return value == "0" || value == "1";
        }
    }
}
