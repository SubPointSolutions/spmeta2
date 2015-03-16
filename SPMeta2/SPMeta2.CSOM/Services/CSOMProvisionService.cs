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
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.Services
{
    public class CSOMProvisionService : ModelServiceBase
    {
        #region constructors

        public CSOMProvisionService()
        {
            ServiceContainer.Instance.RegisterService(typeof(CSOMTokenReplacementService), new CSOMTokenReplacementService());

            RegisterModelHandlers();
            CheckSharePointRuntimeVersion();
        }

        private void CheckSharePointRuntimeVersion()
        {
            // Require minimum SP2013 SP1 which is 15.0.4569.1000
            // We need 15.0.4569.1000 as this allows to create content types with particular ID
            // If current assembly version is lover than 15.0.4569.1000, then we gonna have missing field exception on ContentTypeCreationInformation.Id
            // http://blogs.technet.com/b/steve_chen/archive/2013/03/26/3561010.aspx
            var minimalVersion = new Version("15.0.4569.1000");

            var spAssembly = typeof(Field).Assembly;
            var spAssemblyFileVersion = FileVersionInfo.GetVersionInfo(spAssembly.Location);

            var versionInfo = new Version(spAssemblyFileVersion.ProductVersion);

            TraceService.InformationFormat((int)LogEventId.ModelProcessing,
                "CSOM - CheckSharePointRuntimeVersion. Required minimal version :[{0}]. Current version: [{1}] Location: [{2}]",
                new object[]
                {
                    minimalVersion,
                    spAssemblyFileVersion,
                    spAssemblyFileVersion.ProductVersion
                });

            if (versionInfo < minimalVersion)
            {
                TraceService.Error((int)LogEventId.ModelProcessing, "CSOM - CheckSharePointRuntimeVersion failed. Throwing SPMeta2NotSupportedException");

                var exceptionMessage = string.Empty;

                exceptionMessage += string.Format("SPMeta2.CSOM.dll requires at least SP2013 SP1 runtime ({0}).{1}", minimalVersion, Environment.NewLine);
                exceptionMessage += string.Format(" Current Microsoft.SharePoint.Client.dll version:[{0}].{1}", spAssemblyFileVersion.ProductVersion, Environment.NewLine);
                exceptionMessage += string.Format(" Current Microsoft.SharePoint.Client.dll location:[{0}].{1}", spAssembly.Location, Environment.NewLine);

                throw new SPMeta2NotSupportedException(exceptionMessage);
            }
        }

        private void RegisterModelHandlers()
        {
            RegisterModelHandlers(typeof(FieldModelHandler).Assembly);
        }

        #endregion

        #region methods

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is CSOMModelHostBase))
                throw new ArgumentException("model host for CSOM needs to be inherited from CSOMModelHostBase");

            var clientContext = (modelHost as CSOMModelHostBase).HostClientContext;

            PreloadProperties(clientContext);

            // TODO, check clientContext.ServerLibraryVersion to make sure it's >= SP2013 SP

            base.DeployModel(modelHost, model);
        }

        private static void PreloadProperties(ClientContext clientContext)
        {
            var needQuery = false;

            if (!clientContext.Site.IsPropertyAvailable("ServerRelativeUrl"))
            {
                clientContext.Load(clientContext.Site, s => s.ServerRelativeUrl);
                clientContext.ExecuteQueryWithTrace();
            }

            if (!clientContext.Web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                clientContext.Load(clientContext.Web, w => w.ServerRelativeUrl);
                clientContext.ExecuteQueryWithTrace();
            }
        }

        public override void RetractModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is CSOMModelHostBase))
                throw new ArgumentException("model host for CSOM needs to be inherited from CSOMModelHostBase");

            base.RetractModel(modelHost, model);
        }

        #endregion
    }

    public static class CSOMProvisionServiceExtensions
    {
        public static void DeploySiteModel(this CSOMProvisionService modelHost, ClientContext context, ModelNode model)
        {
            modelHost.DeployModel(new SiteModelHost(context), model);
        }

        public static void DeployWebModel(this CSOMProvisionService modelHost, ClientContext context, ModelNode model)
        {
            modelHost.DeployModel(new WebModelHost(context), model);
        }
    }
}
