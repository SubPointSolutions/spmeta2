using SPMeta2.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Services.Impl
{
    public class DefaultSSOMWebApplicationPropertyBagStorage : SharePointPersistenceStorageServiceBase
    {
        #region consturctors

        public DefaultSSOMWebApplicationPropertyBagStorage()
        {

        }

        public DefaultSSOMWebApplicationPropertyBagStorage(SPWebApplication webApplication)
        {
            CurrentWebApplication = webApplication;
        }

        #endregion

        #region properties

        public SPWebApplication CurrentWebApplication { get; set; }

        public override List<Type> TargetDefinitionTypes
        {
            get
            {
                var result = new List<Type>();

                result.Add(typeof(WebApplicationDefinition));

                return result;
            }
            set { }
        }


        #endregion

        #region methods

        protected Hashtable ExtractProperties()
        {
            return CurrentWebApplication.Properties;
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

            CurrentWebApplication.Update();
        }

        public override void InitialiseFromModelHost(object modelHost)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());

            this.CurrentWebApplication = typedModelHost.HostWebApplication;
        }

        #endregion
    }
}
