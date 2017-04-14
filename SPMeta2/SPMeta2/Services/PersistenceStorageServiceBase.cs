using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services
{
    public abstract class PersistenceStorageServiceBase
    {
        #region methods

        public abstract byte[] LoadObject(string objectId);
        public abstract void SaveObject(string objectId, byte[] data);

        #endregion
    }

    public abstract class SharePointPersistenceStorageServiceBase : PersistenceStorageServiceBase
    {
        public abstract void InitialiseFromModelHost(object modelHost);
        public abstract List<Type> TargetDefinitionTypes { get; set; }
    }
}
