using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Utils;
using System.Reflection;
using System.Diagnostics;
using SPMeta2.CSOM.Services.Impl;
using SPMeta2.Exceptions;
using SPMeta2.Services.Impl;
using SPMeta2.Common;
using SPMeta2.Interfaces;

namespace SPMeta2.CSOM.Services
{
    public class CSOMIncrementalProvisionService : CSOMProvisionService, IIncrementalProvisionService
    {
        #region constructors

        public CSOMIncrementalProvisionService()
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
