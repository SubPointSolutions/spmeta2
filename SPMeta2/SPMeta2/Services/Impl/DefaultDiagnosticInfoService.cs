using SPMeta2.Definitions;
using SPMeta2.Diagnostic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace SPMeta2.Services.Impl
{
    public class DefaultDiagnosticInfoService
    {
        public virtual DiagnosticInfo GetDiagnosticInfo()
        {
            var result = new DiagnosticInfo();

            FillSPMeta2Info(result);

            FillSharePointSSOMInfo(result);
            FillSharePointCSOMInfo(result);

            return result;
        }

        private void FillSharePointSSOMInfo(DiagnosticInfo result)
        {
            // hitting SSOM assembly?
            WithSafeAccess(() =>
            {
                var spType = AppDomain.CurrentDomain
                                        .GetAssemblies()
                                        .SelectMany(a => a.GetTypes())
                                        .FirstOrDefault(t => t.FullName == "Microsoft.SharePoint.SPField");

                if (spType != null)
                {
                    result.IsSSOMDetected = true;

                    result.SSOMFileLocation = spType.Assembly.Location;

                    var fileInfo = FileVersionInfo.GetVersionInfo(result.SSOMFileLocation);

                    result.SSOMFileVersion = fileInfo.FileVersion;
                    result.SSOMProductVersion = fileInfo.ProductVersion;
                }
                else
                {
                    result.IsSSOMDetected = false;
                }
            }, e =>
            {
                result.SSOMFileVersion = e.ToString();
                result.SSOMProductVersion = e.ToString();
            });
        }

        private void FillSharePointCSOMInfo(DiagnosticInfo result)
        {
            // hitting CSOM assembly?
            WithSafeAccess(() =>
            {
                var spType = AppDomain.CurrentDomain
                                        .GetAssemblies()
                                        .SelectMany(a => a.GetTypes())
                                        .FirstOrDefault(t => t.FullName == "Microsoft.SharePoint.Client.Field");

                if (spType != null)
                {
                    result.IsCSOMDetected = true;

                    result.CSOMFileLocation = spType.Assembly.Location;

                    var fileInfo = FileVersionInfo.GetVersionInfo(result.CSOMFileLocation);

                    result.CSOMFileVersion = fileInfo.FileVersion;
                    result.CSOMProductVersion = fileInfo.ProductVersion;
                }
                else
                {
                    result.IsCSOMDetected = false;
                }
            }, e =>
            {
                result.SSOMFileVersion = e.ToString();
                result.SSOMProductVersion = e.ToString();
            });
        }

        protected virtual void FillSPMeta2Info(DiagnosticInfo result)
        {
            // assembly location
            WithSafeAccess(() =>
            {
                result.SPMeta2FileLocation = typeof(FieldDefinition).Assembly.Location;
            }, e =>
            {
                result.SPMeta2FileLocation = e.ToString();
            });

            // assembly file version
            WithSafeAccess(() =>
            {
                var fileInfo = FileVersionInfo.GetVersionInfo(result.SPMeta2FileLocation);

                result.SPMeta2FileVersion = fileInfo.FileVersion;
                result.SPMeta2ProductVersion = fileInfo.ProductVersion;
            }, e =>
            {
                result.SPMeta2FileVersion = e.ToString();
                result.SPMeta2ProductVersion = e.ToString();
            });
        }

        protected void WithSafeAccess(Action action, Action<Exception> err)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                err(e);
            }
        }
    }
}
