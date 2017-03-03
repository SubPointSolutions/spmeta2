using SPMeta2.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.Services.Impl
{
    public class DefaultSSOMWebPropertyBagStorage : SharePointPersistenceStorageServiceBase
    {
        #region consturctors

        public DefaultSSOMWebPropertyBagStorage()
        {

        }

        public DefaultSSOMWebPropertyBagStorage(SPWeb web)
        {
            CurrentWeb = web;
        }

        #endregion

        #region properties

        public SPWeb CurrentWeb { get; set; }

        #endregion

        #region methods

        protected Hashtable ExtractProperties()
        {
            return CurrentWeb.AllProperties;
        }

        public override List<Type> TargetDefinitionTypes
        {
            get
            {
                var result = new List<Type>();

                result.Add(typeof(SiteDefinition));
                result.Add(typeof(WebDefinition));

                return result;
            }
            set { }
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

            CurrentWeb.Update();
        }

        public override void InitialiseFromModelHost(object modelHost)
        {
            var csomModelHost = modelHost.WithAssertAndCast<SSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            if (csomModelHost is WebModelHost)
            {
                var webModelHost = csomModelHost as WebModelHost;

                this.CurrentWeb = webModelHost.HostWeb;
            }
            else if (csomModelHost is SiteModelHost)
            {
                var webModelHost = csomModelHost as SiteModelHost;

                this.CurrentWeb = webModelHost.HostSite.RootWeb;
            }
            else
            {
                throw new SPMeta2Exception(string.Format("Unsuported model host type:[{0}]", modelHost.GetType()));
            }
        }

        #endregion
    }
}
