using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Interfaces;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System;
using System.Reflection;

namespace SPMeta2.SSOM.Services
{
    public class SSOMIncrementalProvisionService : SSOMProvisionService, IIncrementalProvisionService
    {
        #region constructors

        public SSOMIncrementalProvisionService()
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
