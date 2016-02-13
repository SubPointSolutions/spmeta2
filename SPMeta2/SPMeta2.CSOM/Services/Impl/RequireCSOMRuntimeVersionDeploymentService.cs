using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services;

namespace SPMeta2.CSOM.Services.Impl
{
    public class RequireCSOMRuntimeVersionDeploymentService : PreDeploymentServiceBase
    {
        #region properties

        public static Version MinimalVersion = new Version("15.0.4569.1000");

        #endregion

        #region methods

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            CheckCSOMRuntimeVersion();
        }

        protected virtual void CheckCSOMRuntimeVersion()
        {
            // Require minimum SP2013 SP1 which is 15.0.4569.1000
            // We need 15.0.4569.1000 as this allows to create content types with particular ID
            // If current assembly version is lover than 15.0.4569.1000, then we gonna have missing field exception on ContentTypeCreationInformation.Id
            // http://blogs.technet.com/b/steve_chen/archive/2013/03/26/3561010.aspx

            var spAssembly = typeof(Field).Assembly;
            var spAssemblyFileVersion = FileVersionInfo.GetVersionInfo(spAssembly.Location);

            var versionInfo = new Version(spAssemblyFileVersion.ProductVersion);

            TraceService.InformationFormat((int)LogEventId.ModelProcessing,
                "CSOM - CheckSharePointRuntimeVersion. Required minimal version :[{0}]. Current version: [{1}] Location: [{2}]",
                new object[]
                {
                    MinimalVersion,
                    spAssemblyFileVersion,
                    spAssemblyFileVersion.ProductVersion
                });

            if (versionInfo < MinimalVersion)
            {
                TraceService.Error((int)LogEventId.ModelProcessing, "CSOM - CheckSharePointRuntimeVersion failed. Throwing SPMeta2NotSupportedException");

                var exceptionMessage = string.Empty;

                exceptionMessage += string.Format("SPMeta2.CSOM.dll requires at least SP2013 SP1 runtime ({0}).{1}", MinimalVersion, Environment.NewLine);
                exceptionMessage += string.Format(" Current Microsoft.SharePoint.Client.dll version:[{0}].{1}", spAssemblyFileVersion.ProductVersion, Environment.NewLine);
                exceptionMessage += string.Format(" Current Microsoft.SharePoint.Client.dll location:[{0}].{1}", spAssembly.Location, Environment.NewLine);

                throw new SPMeta2NotSupportedException(exceptionMessage);
            }
        }

        #endregion
    }
}
