using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Diagnostic
{
    public class DiagnosticInfo
    {
        #region propertis
        public string SPMeta2FileLocation { get; set; }
        public string SPMeta2FileVersion { get; set; }
        public string SPMeta2ProductVersion { get; set; }


        public string SSOMFileLocation { get; set; }
        public string SSOMFileVersion { get; set; }
        public string SSOMProductVersion { get; set; }


        public string CSOMFileLocation { get; set; }
        public string CSOMFileVersion { get; set; }
        public string CSOMProductVersion { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            var toString = new ToStringResultRaw(base.ToString())
                         .AddRawPropertyValue("SPMeta2FileVersion", SPMeta2FileVersion)
                         .AddRawPropertyValue("SPMeta2FileLocation", SPMeta2FileLocation);

            if (IsCSOMDetected)
            {
                toString.AddRawPropertyValue("CSOMFileVersion", CSOMFileVersion);
                toString.AddRawPropertyValue("CSOMFileLocation", CSOMFileLocation);
            }
            else
            {
                toString.AddRawPropertyValue("IsCSOMDetected", IsCSOMDetected);
            }

            if (IsSSOMDetected)
            {
                toString.AddRawPropertyValue("SSOMFileVersion", SSOMFileVersion);
                toString.AddRawPropertyValue("SSOMFileLocation", SSOMFileLocation);
            }
            else
            {
                toString.AddRawPropertyValue("IsSSOMDetected", IsSSOMDetected);
            }

            return toString.ToString(Environment.NewLine);
        }

        #endregion

        public bool IsSSOMDetected { get; set; }

        public bool IsCSOMDetected { get; set; }
    }
}
