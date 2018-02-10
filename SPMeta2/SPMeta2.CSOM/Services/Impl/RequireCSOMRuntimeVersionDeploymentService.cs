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
using System.Reflection;

namespace SPMeta2.CSOM.Services.Impl
{
    public class RequireCSOMRuntimeVersionDeploymentService : PreDeploymentServiceBase
    {
        #region static

        static RequireCSOMRuntimeVersionDeploymentService()
        {
            SP2013MinimalVersion = new Version("15.0.4569.1000");
        }

        #endregion

        #region properties

        [Obsolete("Always considered to be SP2013 CSOM minimal version. Use SP2013MinimalVersion and the rest of the props for the future.")]

        public static Version MinimalVersion = new Version("15.0.4569.1000");

        /// <summary>
        /// Not used. Reversed for the future.
        /// </summary>
        public static Version SP2010MinimalVersion { get; set; }

        /// <summary>
        /// Used to detecting minimal required SP2013 CSOM runtime.
        /// Replacement for old MinimalVersion (always for 2013)
        /// </summary>
        public static Version SP2013MinimalVersion { get; set; }

        /// <summary>
        /// Not used. Reversed for the future.
        /// </summary>
        public static Version SP2016MinimalVersion { get; set; }


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

            var spAssembly = GetCSOMRuntimeAssembly();
            var versionInfo = GetCSOMRuntimeVersion();

            var assemblyLocation = string.Empty;

            // RequireCSOMRuntimeVersionDeploymentService does not work under Azure Funtions #962
            // https://github.com/SubPointSolutions/spmeta2/issues/962
            try
            {
                assemblyLocation = spAssembly.Location;
            }
            catch (Exception assemblyLocationLoadException)
            {
                assemblyLocation = string.Format("Coudn't load .Location property. Exception was:[{0}]", assemblyLocationLoadException);
            }

            TraceService.InformationFormat((int)LogEventId.ModelProcessing,
                "CSOM - CheckSharePointRuntimeVersion. Required minimal version :[{0}]. Current version: [{1}] Location: [{2}]",
                new object[]
                {
                   MinimalVersion,
                   versionInfo,
                   assemblyLocation
                });

            if (versionInfo.Major == 14)
            {
                // TODO, SP2010 check later
            }
            else if (versionInfo.Major == 15)
            {
                if (versionInfo < SP2013MinimalVersion)
                {
                    TraceService.Error((int)LogEventId.ModelProcessing, "CSOM - CheckSharePointRuntimeVersion failed. Throwing SPMeta2NotSupportedException");

                    var exceptionMessage = string.Empty;

                    exceptionMessage += string.Format("SPMeta2.CSOM.dll requires at least SP2013 SP1 runtime ({0}).{1}", MinimalVersion, Environment.NewLine);
                    exceptionMessage += string.Format(" Current Microsoft.SharePoint.Client.dll version:[{0}].{1}", versionInfo.Major, Environment.NewLine);
                    exceptionMessage += string.Format(" Current Microsoft.SharePoint.Client.dll location:[{0}].{1}", assemblyLocation, Environment.NewLine);

                    throw new SPMeta2NotSupportedException(exceptionMessage);
                }
            }
            else if (versionInfo.Major == 16)
            {
                // TODO, SP2016 check later
            }
        }

        protected virtual Assembly GetCSOMRuntimeAssembly()
        {
            return typeof(Field).Assembly;
        }

        protected virtual Version GetCSOMRuntimeVersion()
        {
            var spAssembly = GetCSOMRuntimeAssembly();

            return GetAssemblyFileVersion(spAssembly);
        }

        public virtual Version GetAssemblyFileVersion(Assembly assembly)
        {
            var fileVersionAttribute = Attribute.GetCustomAttribute(
                                            assembly,
                                            typeof(AssemblyFileVersionAttribute), false) as AssemblyFileVersionAttribute;

            // paranoic about regression caused by MS changes in the future
            if (fileVersionAttribute == null)
            {
                throw new SPMeta2Exception(string.Format(
                        "Cannot find AssemblyFileVersionAttribute in assembly:[{0}]",
                        assembly.FullName));
            }

            if (string.IsNullOrEmpty(fileVersionAttribute.Version))
            {
                throw new SPMeta2Exception(string.Format(
                        "Found AssemblyFileVersionAttribute but .Version is null or empty. Assembly:[{0}]",
                        assembly.FullName));
            }


            return new Version(fileVersionAttribute.Version);
        }

        #endregion
    }
}
