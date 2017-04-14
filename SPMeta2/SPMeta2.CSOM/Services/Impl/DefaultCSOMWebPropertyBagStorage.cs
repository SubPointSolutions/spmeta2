using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Services.Impl
{
    public class DefaultCSOMWebPropertyBagStorage : SharePointPersistenceStorageServiceBase
    {
        #region constructors

        public DefaultCSOMWebPropertyBagStorage()
        {

        }

        public DefaultCSOMWebPropertyBagStorage(Web web)
        {
            // follow CSOM PropertyModelHandler implementation
            // codebase is better be merged later

            CurrentWeb = web;
            CurrentContext = web.Context;
        }

        #endregion

        #region properties

        protected Web CurrentWeb { get; private set; }
        protected ClientRuntimeContext CurrentContext { get; private set; }

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

        #endregion


        #region methods

        protected PropertyValues ExtractProperties()
        {
            var result = CurrentWeb.AllProperties;

            CurrentContext.Load(result);
            CurrentContext.ExecuteQueryWithTrace();

            return result;
        }

        public override byte[] LoadObject(string objectId)
        {
            var key = objectId;

            var properties = ExtractProperties();
            var currentValue = properties.FieldValues.ContainsKey(key) ? properties[key] : null;

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
            CurrentContext.ExecuteQueryWithTrace();
        }

        public override void InitialiseFromModelHost(object modelHost)
        {
            var csomModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            if (csomModelHost is WebModelHost)
            {
                var webModelHost = csomModelHost as WebModelHost;

                this.CurrentWeb = webModelHost.HostWeb;
                this.CurrentContext = webModelHost.HostWeb.Context;
            }
            else if (csomModelHost is SiteModelHost)
            {
                var webModelHost = csomModelHost as SiteModelHost;

                this.CurrentWeb = webModelHost.HostWeb;
                this.CurrentContext = webModelHost.HostWeb.Context;
            }
            else
            {
                throw new SPMeta2Exception(string.Format("Unsuported model host type:[{0}]", modelHost.GetType()));
            }
        }

        #endregion


    }
}
