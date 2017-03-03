using SPMeta2.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Services.Impl
{
    public class DefaultSSOMFarmPropertyBagStorage : PersistenceStorageServiceBase
    {
        #region consturctors

        public DefaultSSOMFarmPropertyBagStorage(SPFarm farm)
        {
            CurrentFarm = farm;
        }

        #endregion

        #region properties

        public SPFarm CurrentFarm { get; set; }

        #endregion

        #region methods

        protected Hashtable ExtractProperties()
        {
            return CurrentFarm.Properties;
        }

        public override byte[] LoadObject(string objectId)
        {
            var key = objectId;

            var properties = ExtractProperties();
            var currentValue = properties[key];

            var dataString = ConvertUtils.ToString(currentValue);

            if (!string.IsNullOrEmpty(dataString))
                return Encoding.UTF8.GetBytes(dataString);

            return null;
        }

        public override void SaveObject(string objectId, byte[] data)
        {
            var key = objectId;

            var properties = ExtractProperties();
            var dataString = Encoding.UTF8.GetString(data);

            properties[key] = dataString;

            CurrentFarm.Update();
        }

        #endregion
    }
}
