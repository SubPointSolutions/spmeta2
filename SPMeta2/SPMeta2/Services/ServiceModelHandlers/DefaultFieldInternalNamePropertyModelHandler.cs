using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public class DefaultFieldInternalNamePropertyModelHandler : TypedDefinitionModelHandlerBase<FieldDefinition>
    {
        protected override void ProcessDefinition(object modelHost, FieldDefinition model)
        {
            // Enhance built-in validation - FieldDefinition.InternalName can't be more than 32 chars #698
            // https://github.com/SubPointSolutions/spmeta2/issues/698

            var internalName = model.InternalName;

            // not null cause dep lookup might go with empty internal name
            // validation on empty-required MUST not be included here, it is done in the other handlers
            if (!string.IsNullOrEmpty(internalName)
                && internalName.Length > 32)
            {
                throw new SPMeta2ModelValidationException(
                   string.Format("InternalName must be leaa than 32 characters:[{1}]. Definition:[{0}]",
                   model, model.InternalName));
            }
        }
    }
}
