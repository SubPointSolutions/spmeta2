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

            AutoDetectSharePointPersistenceStorage = false;
        }

        #endregion

        #region properties

        public bool AutoDetectSharePointPersistenceStorage { get; set; }
        public ModelHash PreviousModelHash { get; set; }

        public Type CustomModelTreeTraverseServiceType { get; set; }

        public List<PersistenceStorageServiceBase> PersistenceStorages { get; set; }

        #endregion

        public static IncrementalProvisionConfig Default
        {
            get
            {
                var config = new IncrementalProvisionConfig
                {
                    CustomModelTreeTraverseServiceType = typeof(DefaultIncrementalModelTreeTraverseService)
                };

                return config;
            }
        }

        public static IncrementalProvisionConfig DefaultFileSystem
        {
            get
            {
                var config = new IncrementalProvisionConfig
                {
                    CustomModelTreeTraverseServiceType = typeof(DefaultIncrementalModelTreeTraverseService)
                };

                config.PersistenceStorages.Add(new DefaultFileSystemPersistenceStorage());

                return config;
            }
        }

        public static IncrementalProvisionConfig DefaultSharePoint
        {
            get
            {
                var config = new IncrementalProvisionConfig
                {
                    CustomModelTreeTraverseServiceType = typeof(DefaultIncrementalModelTreeTraverseService)
                };

                config.AutoDetectSharePointPersistenceStorage = true;

                return config;
            }
        }
    }
}
