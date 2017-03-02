using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.SSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;

using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Common;
using SPMeta2.Interfaces;

namespace SPMeta2.SSOM.Standard.Services
{
    public class StandardSSOMIncrementalProvisionService : SSOMProvisionService, IIncrementalProvisionService
    {
         #region constructors

        public StandardSSOMIncrementalProvisionService()
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
