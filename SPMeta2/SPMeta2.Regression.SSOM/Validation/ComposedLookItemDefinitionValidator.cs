using System;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ComposedLookItemDefinitionValidator : ListItemDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ComposedLookItemDefinition); }
        }

        #region methods

        protected override void ValidateProperties(SPListItem item, ListItemDefinition definition)
        {
            base.ValidateProperties(item, definition);

            var typedDefinition = definition as ComposedLookItemDefinition;
            var assert = ServiceFactory.AssertService
                             .NewAssert(typedDefinition, item)
                              .ShouldNotBeNull(item);
            // TODO
        }

        #endregion
    }
}
