using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Services.Impl.Validation
{
    public class DiscussionBoardListFieldDefinitionValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<DiscussionBoardListDefinitionValidationModelHandler>
    {
        #region constructors

        public DiscussionBoardListFieldDefinitionValidationService()
        {
            this.Title = "List definition validator for discussion boards";
            this.Description = "Ensures discussion boards have content types enabled";
        }

        #endregion
    }
}
