using SPMeta2.Definitions.Fields;
using SPMeta2.Exceptions;
using SPMeta2.Services.ServiceModelHandlers;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Standard.Services.ServiceModelHandlers
{
    public class PublishingPageDefinitionValidationModelHandler : TypedDefinitionModelHandlerBase<PublishingPageDefinition>
    {
        protected override void ProcessDefinition(object modelHost, PublishingPageDefinition model)
        {
            // https://github.com/SubPointSolutions/spmeta2/issues/791
            // Enhance PublishingPageDefinition - add PageLayoutFileName property validation #791

            if (!string.IsNullOrEmpty(model.PageLayoutFileName))
            {
                var isValidValue = IsValidPageLayoutFileNameValue(model);

                if (!isValidValue)
                {
                    throw new SPMeta2ModelValidationException(
                        string.Format("PageLayoutFileName value should ends with '.aspx' Current value:[{1}] Definition:[{0}]",
                                    model, model.PageLayoutFileName));
                }
            }
        }

        protected virtual bool IsValidPageLayoutFileNameValue(PublishingPageDefinition model)
        {
            return model.PageLayoutFileName.ToUpper().EndsWith(".ASPX");
        }
    }
}
