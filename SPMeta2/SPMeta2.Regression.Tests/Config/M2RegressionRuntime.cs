using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Config
{
    public static class M2RegressionRuntime
    {
        public static Version CurrentAPIVersion = new Version(1, 2, 0, 0);

        public static bool IsV11
        {
            get
            {
                return CurrentAPIVersion.Major == 1
                       && CurrentAPIVersion.Minor == 1;
            }
        }

        public static bool IsV12
        {
            get
            {
                return CurrentAPIVersion.Major == 1
                       && CurrentAPIVersion.Minor == 2;
            }
        }
    }

    public static class M2Consts
    {
        public static Version APIv11 = new Version(1, 2, 0, 0);
        public static Version APIv12 = new Version(1, 1, 0, 0);
    }
}
