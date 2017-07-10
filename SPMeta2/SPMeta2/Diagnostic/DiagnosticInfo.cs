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
            var toString = new ToStringResult<DiagnosticInfo>(this, base.ToString())
                         .AddPropertyValue(p => p.SPMeta2FileVersion)
                         .AddPropertyValue(p => p.SPMeta2FileLocation);

            if (IsCSOMDetected)
            {
                toString.AddPropertyValue(p => p.CSOMFileVersion);
                toString.AddPropertyValue(p => p.CSOMFileLocation);
            }
            else
            {
                toString.AddPropertyValue(p => p.IsCSOMDetected);
            }

            if (IsSSOMDetected)
            {
                toString.AddPropertyValue(p => p.SSOMFileVersion);
                toString.AddPropertyValue(p => p.SSOMFileLocation);
            }
            else
            {
                toString.AddPropertyValue(p => p.IsSSOMDetected);
            }

            return toString.ToString(Environment.NewLine);
        }

        #endregion

        public bool IsSSOMDetected { get; set; }

        public bool IsCSOMDetected { get; set; }
    }
}
