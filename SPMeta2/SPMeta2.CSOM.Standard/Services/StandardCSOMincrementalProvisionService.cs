using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Services.Impl;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Services;
using SPMeta2.Interfaces;


namespace SPMeta2.CSOM.Standard.Services
{
    public class StandardCSOMIncrementalProvisionService : StandardCSOMProvisionService, IIncrementalProvisionService
    {
        #region constructors

        public StandardCSOMIncrementalProvisionService()
        {
            this.SetIncrementalProvisionMode();
        }

        #endregion

        #region properties

        protected virtual DefaultIncrementalModelTreeTraverseService TypedModelTreeTraverseService
        {
            get
            {
                return ModelTraverseService as DefaultIncrementalModelTreeTraverseService;
            }
        }

        public ModelHash PreviousModelHash
        {
            get
            {
                return TypedModelTreeTraverseService.PreviousModelHash;
            }
            set
            {
                TypedModelTreeTraverseService.PreviousModelHash = value;
            }
        }
        public ModelHash CurrentModelHash
        {
            get
            {
                return TypedModelTreeTraverseService.CurrentModelHash;
            }
            set
            {

            }
        }

        #endregion

        #region methods


        #endregion
    }
}
