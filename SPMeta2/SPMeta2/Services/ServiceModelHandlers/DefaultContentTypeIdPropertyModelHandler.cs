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
    public class DefaultContentTypeIdPropertyModelHandler : TypedDefinitionModelHandlerBase<ContentTypeDefinition>
    {
        protected override void ProcessDefinition(object modelHost, ContentTypeDefinition model)
        {
            // skipping check
            if (string.IsNullOrEmpty(model.ParentContentTypeId)
                && !string.IsNullOrEmpty(model.ParentContentTypeName))
            {
                return;
            }

            // https://github.com/SubPointSolutions/spmeta2/issues/689
            var contentTypeId = model.GetContentTypeId();

            // crazy and impossible case, but..
            if (string.IsNullOrEmpty(contentTypeId))
            {
                throw new SPMeta2ModelValidationException(
                    string.Format("contentTypeId is NULL or empty. Definition:[{0}]", model));
            }

            // content type ID should start with 0x
            // invalid cases happen than we define an incorrect parent content type ID
            // pain to troubleshoot and handle, and the re-create a site collection :(
            if (!contentTypeId.ToUpper().StartsWith("0X"))
            {
                throw new SPMeta2ModelValidationException(
                    string.Format("contentTypeId value is invalid:[{1}]. Definition:[{0}]", model, contentTypeId));
            }

            // turned out that the 0x must be in the lower case :)
            // https://github.com/SubPointSolutions/spmeta2/issues/754
            if (!contentTypeId.StartsWith("0x"))
            {
                throw new SPMeta2ModelValidationException(
                    string.Format("contentTypeId value must start with '0x' in lower case:[{1}]. Definition:[{0}]", model, contentTypeId));
            }
        }
    }
}
