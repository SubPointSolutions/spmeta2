using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public class DiscussionBoardListDefinitionValidationModelHandler :
        TypedDefinitionModelHandlerBase<ListDefinition>
    {
        #region propeties

        private static string ErrorMessage = "ListDefinition for discussion board must have content types enabled - https://github.com/SubPointSolutions/spmeta2/issues/879";

        #endregion

        #region methods

        protected override void ProcessDefinition(object modelHost, ListDefinition model)
        {
            // Enhance ListDefinition provision - add discussion board validation #879
            // https://github.com/SubPointSolutions/spmeta2/issues/879

            // All discussion boards must have content types enabled
            // https://github.com/SubPointSolutions/spmeta2/issues/879

            if (model.TemplateType == BuiltInListTemplateTypeId.DiscussionBoard
                && !model.ContentTypesEnabled)
            {
                throw new SPMeta2ModelValidationException(ErrorMessage);
            }

            if (model.TemplateName == BuiltInListTemplates.DiscussionBoard.InternalName
                && !model.ContentTypesEnabled)
            {
                throw new SPMeta2ModelValidationException(ErrorMessage);
            }
        }

        #endregion
    }
}
