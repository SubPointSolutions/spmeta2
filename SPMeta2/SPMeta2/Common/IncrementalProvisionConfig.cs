using SPMeta2.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Services;

namespace SPMeta2.Common
{



    public class IncrementalProvisionConfig
    {
        #region constructors

        public IncrementalProvisionConfig()
        {
            PreviousModelHash = new ModelHash();
            PersistenceStorages = new List<PersistenceStorageServiceBase>();
        }

        #endregion

        #region properties
        public ModelHash PreviousModelHash { get; set; }

        public Type CustomModelTreeTraverseServiceType { get; set; }

        public List<PersistenceStorageServiceBase> PersistenceStorages { get; set; }

        #endregion

        public static IncrementalProvisionConfig Default
        {
            get
            {
                return new IncrementalProvisionConfig
                {
                    CustomModelTreeTraverseServiceType = typeof(DefaultIncrementalModelTreeTraverseService)
                };
            }
        }
    }
}
